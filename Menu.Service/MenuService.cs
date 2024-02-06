// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service
{
    using System.Linq;
    using CloudStorage.Service.Interfaces;
    using Infrastructure.Core.Exceptions;
    using Infrastructure.Core.Models;
    using Infrastructure.Database;
    using Menu.Service.Exceptions;
    using Menu.Service.Interfaces;
    using Menu.Service.Models.DTOs;
    using Messaging.Service.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class MenuService : IMenuService
    {
        private readonly ICloudStorageService cloudStorageService;
        private readonly INewsMessagingService newsMessagingService;
        private readonly IDbContextFactory<MenuServiceDatabaseContext> dbCxtFactory;

        public MenuService(
            ICloudStorageService cloudStorageService,
            INewsMessagingService newsMessagingService,
            IDbContextFactory<MenuServiceDatabaseContext> dbCxtFactory)
        {
            this.cloudStorageService = cloudStorageService;
            this.newsMessagingService = newsMessagingService;
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<MenuItemResponse>> GetMenu(
            int offset = 0,
            int count = 100,
            IEnumerable<int>? categories = default,
            IEnumerable<int>? ids = default,
            bool orderDesc = false,
            bool onlyVisible = true)
        {
            try
            {
                await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

                var selectQuery =
                    dbContext.MenuItems.AsNoTracking()
                        .Select(m => new MenuItemResponse
                        {
                            Id = m.Id,
                            Name = m.Name,
                            Description = m.Description,
                            Price = m.Price,
                            Visible = m.Visible,
                            ImageUrl = m.Image.Url,
                            Categories = m.MenuItemCategories
                                .Select(c => c.CategoryId)
                                .ToList(),
                        });

                var whereQuery = selectQuery.Where(x =>
                        (onlyVisible == default || x.Visible == true) &&
                        (ids == default || ids.Any(id => id == x.Id)) &&
                        (categories == default || x.Categories!.Any(x => categories.Any(y => y == x))));

                var orderQuery = orderDesc ?
                    whereQuery.OrderByDescending(x => x.Id) :
                    whereQuery.OrderBy(x => x.Id);

                var pageQuery = orderQuery.Skip(offset).Take(count);

                var menu = await pageQuery.ToListAsync();

                return menu;
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to get Menu Items", ex);
            }
        }

        public async Task<MenuItemResponse> GetMenuItem(int id)
        {
            try
            {
                await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

                var menuItemQuery = dbContext.MenuItems.AsNoTracking()
                    .Select(m => new MenuItemResponse
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Price = m.Price,
                        Visible = m.Visible,
                        ImageUrl = m.Image.Url,
                        Categories = m.MenuItemCategories
                            .Select(c => c.CategoryId)
                            .ToList(),
                    });

                var menuItem = await menuItemQuery.FirstOrDefaultAsync(x => x.Id == id);

                if (menuItem == null)
                {
                    throw new MenuItemNotFoundException($"Not found menuItem with id = {id} while executing GetMenuItem method");
                }

                return menuItem;
            }
            catch (MenuItemNotFoundException ex)
            {
                throw new NotFoundException("Menu Item Not found", ex);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to get Menu Item", ex);
            }
        }

        public async Task<MenuItemResponse> CreateMenuItem(MenuItemDTO newItemDto)
        {
            try
            {
                await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();
                await using var transaction = dbContext.Database.BeginTransaction();

                if (dbContext.Categories.AsNoTracking()
                        .Count(c => newItemDto.Categories.Contains(c.Id))
                        != newItemDto.Categories.Count())
                {
                    throw new InvalidCategoryException();
                }

                var newItem = new MenuItem()
                {
                    Name = newItemDto.Name,
                    Description = newItemDto.Description,
                    Price = newItemDto.Price,
                    Visible = newItemDto.Visible,
                };

                if (newItemDto.Image != default && newItemDto.Image.Length != 0)
                {
                    var newImage = await this.cloudStorageService.UploadFile(newItemDto.Image, "menuItem", "/menuItems");
                    var image = (await dbContext.CloudFiles.AddAsync(newImage)).Entity;

                    newItem.Image = image;
                }

                var menuItem = (await dbContext.MenuItems.AddAsync(newItem)).Entity;
                await dbContext.SaveChangesAsync();

                var menuItemCategories = new List<MenuItemCategory>();
                foreach (var item in newItemDto.Categories)
                {
                    menuItemCategories.Add(
                        new MenuItemCategory
                        {
                            CategoryId = item,
                            MenuItemId = menuItem.Id,
                        });
                }

                await dbContext.MenuItemCategories.AddRangeAsync(menuItemCategories);
                await dbContext.SaveChangesAsync();

                transaction.Commit();

                var menuItemResponse = new MenuItemResponse(newItem);

                this.newsMessagingService.SendMessage(menuItemResponse);

                return menuItemResponse;
            }
            catch (InvalidCategoryException ex)
            {
                throw new BadRequestException("Invalid Menu Item Category", ex);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to create Menu Item", ex);
            }
        }

        public async Task<MenuItemResponse> UpdateMenuItem(int id, MenuItemDTO menuItemDto)
        {
            try
            {
                await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();
                await using var transaction = dbContext.Database.BeginTransaction();

                if (dbContext.Categories.AsNoTracking()
                        .Count(c => menuItemDto.Categories.Contains(c.Id))
                        != menuItemDto.Categories.Count())
                {
                    throw new InvalidCategoryException();
                }

                var menuItem = await dbContext.MenuItems
                    .Include(m => m.Image)
                    .Include(m => m.MenuItemCategories)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (menuItem == default)
                {
                    throw new MenuItemNotFoundException();
                }

                menuItem.Name = menuItemDto.Name ?? menuItem.Name;
                menuItem.Description = menuItemDto.Description ?? menuItem.Description;
                menuItem.Price = menuItemDto.Price != default ? menuItemDto.Price : menuItem.Price;
                menuItem.Visible = menuItemDto.Visible;

                if (menuItemDto.Image != default && menuItemDto.Image.Length != 0)
                {
                    if (menuItem.Image != default)
                    {
                        await this.cloudStorageService.RemoveFile(menuItem.Image.PublicId!, menuItem.Image.ResourceType!);
                        dbContext.CloudFiles.Remove(menuItem.Image);
                    }

                    var newImage = await this.cloudStorageService.UploadFile(menuItemDto.Image, "menuItem", "/menuItems");
                    var image = (await dbContext.CloudFiles.AddAsync(newImage)).Entity;
                    menuItem.Image = image;

                    dbContext.Update(menuItem);
                    await dbContext.SaveChangesAsync();
                    dbContext.ChangeTracker.Clear();
                }

                dbContext.TryUpdateManyToMany(
                    menuItem.MenuItemCategories,
                    menuItemDto.Categories
                    .Select(c => new MenuItemCategory
                    {
                        MenuItemId = menuItem.Id,
                        CategoryId = c,
                    }),
                    x => x.CategoryId);

                dbContext.Update(menuItem);
                await dbContext.SaveChangesAsync();

                transaction.Commit();

                var menuItemResponse = new MenuItemResponse(menuItem);

                return menuItemResponse;
            }
            catch (InvalidCategoryException ex)
            {
                throw new BadRequestException("Invalid Menu Item Category", ex);
            }
            catch (MenuItemNotFoundException ex)
            {
                throw new NotFoundException("Menu Item Not found", ex);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to update Menu Item", ex);
            }
        }
    }
}
