// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Data;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;

    public class MenuService : IMenuService
    {
        private readonly IDbContextFactory<MenuDatabaseContext> dbCxtFactory;

        public MenuService(IDbContextFactory<MenuDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<MenuItem>> GetAllMenu()
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            return await Task.FromResult(dbContext.Menu.ToList());
        }

        public async Task<MenuItem> GetMenuItem(int id)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var menuItem = await dbContext.Menu.FindAsync(id);
            if (menuItem == null)
            {
                throw new NotFoundException($"Not found menuItem with id = {id} while executing GetMenuItem method");
            }

            return menuItem;
        }

        public async Task<MenuItem> CreateMenuItem(MenuItemDto newItemDto)
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

        public async Task<MenuItem> UpdateMenuItem(int id, MenuItemDto newItemDto)
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
