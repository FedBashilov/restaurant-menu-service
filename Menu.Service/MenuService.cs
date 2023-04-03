// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service
{
    using System.Linq;
    using Infrastructure.Core.Models;
    using Infrastructure.Database;
    using Menu.Service.Exceptions;
    using Menu.Service.Models.DTOs;
    using Microsoft.EntityFrameworkCore;

    public class MenuService : IMenuService
    {
        private readonly IDbContextFactory<MenuDatabaseContext> dbCxtFactory;

        public MenuService(IDbContextFactory<MenuDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<MenuItem>> GetMenu(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

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
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var menuItem = await dbContext.Menu.FirstAsync(x => x.Id == id);

            if (menuItem == null)
            {
                throw new NotFoundException($"Not found menuItem with id = {id} while executing GetMenuItem method");
            }

            return menuItem;
        }

        public async Task<MenuItem> CreateMenuItem(MenuItemDTO newItemDto)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();

            var newItem = new MenuItem()
            {
                Name = newItemDto.Name,
                Price = newItemDto.Price,
            };

            var menuItem = dbContext.Menu.Add(newItem).Entity;
            await dbContext.SaveChangesAsync();

            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItem(int id, MenuItemDTO newItemDto)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            if (!dbContext.Menu.Any(x => x.Id == id))
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
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var menuItem = dbContext.Menu.FirstOrDefault(x => x.Id == id);

            if (menuItem != null)
            {
                dbContext.Remove(menuItem);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
