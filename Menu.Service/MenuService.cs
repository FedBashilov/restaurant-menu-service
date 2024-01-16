﻿// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service
{
    using System.Linq;
    using CloudStorage.Service.Interfaces;
    using Infrastructure.Core.Models;
    using Infrastructure.Database;
    using Menu.Service.Interfaces;
    using Menu.Service.Models.DTOs;
    using Microsoft.EntityFrameworkCore;
    using Shared.Exceptions;

    public class MenuService : IMenuService
    {
        private readonly ICloudStorageService cloudStorageService;
        private readonly IDbContextFactory<MenuServiceDatabaseContext> dbCxtFactory;

        public MenuService(
            ICloudStorageService cloudStorageService,
            IDbContextFactory<MenuServiceDatabaseContext> dbCxtFactory)
        {
            this.cloudStorageService = cloudStorageService;
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<MenuItem>> GetMenu(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

            var selectQuery = onlyVisible ?
                dbContext.Menu.Where(x => x.Visible == true) :
                dbContext.Menu;

            var orderQuery = orderDesc ?
                selectQuery.OrderByDescending(x => x.Id) :
                selectQuery.OrderBy(x => x.Id);

            var pageQuery = orderQuery.Skip(offset).Take(count);

            var menu = await pageQuery.ToListAsync();

            return menu;
        }

        public async Task<MenuItem> GetMenuItem(int id)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

            var menuItem = await dbContext.Menu.FirstOrDefaultAsync(x => x.Id == id);

            if (menuItem == null)
            {
                throw new NotFoundException($"Not found menuItem with id = {id} while executing GetMenuItem method");
            }

            return menuItem;
        }

        public async Task<MenuItem> CreateMenuItem(MenuItemDTO newItemDto)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

            var newItem = new MenuItem()
            {
                Name = newItemDto.Name,
                Price = newItemDto.Price,
            };

            var menuItem = (await dbContext.Menu.AddAsync(newItem)).Entity;

            if (newItemDto.Image != default && newItemDto.Image.Length != 0)
            {
                var imageUrl = await this.cloudStorageService.UploadFile(newItemDto.Image, "menuItem", "/menuItems");
                menuItem.ImageUrl = imageUrl.ToString();
            }

            await dbContext.SaveChangesAsync();

            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItem(int id, MenuItemDTO newItemDto)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();
            if (!await dbContext.Menu.AnyAsync(x => x.Id == id))
            {
                throw new NotFoundException();
            }

            var newItem = new MenuItem()
            {
                Id = id,
                Name = newItemDto.Name,
                Price = newItemDto.Price,
            };

            var menuItem = dbContext.Menu.Update(newItem).Entity;
            await dbContext.SaveChangesAsync();

            return menuItem;
        }

        public async Task DeleteMenuItem(int id)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();
            var menuItem = await dbContext.Menu.FirstOrDefaultAsync(x => x.Id == id);

            if (menuItem != null)
            {
                dbContext.Menu.Remove(menuItem);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
