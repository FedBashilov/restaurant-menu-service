// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;

    public class MenuService : IMenuService
    {
        private readonly IDbContextFactory<DatabaseContext> dbCxtFactory;

        public MenuService(IDbContextFactory<DatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public List<MenuItem> GetAllMenu()
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            return dbContext.Menu.ToList();
        }

        public MenuItem GetMenuItem(int id)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var menuItem = dbContext.Menu.Find(id);
            if (menuItem == null)
            {
                throw new NotFoundException($"Not found menuItem with id = {id} while executing GetMenuItem method");
            }

            return menuItem;
        }

        public MenuItem CreateMenuItem(MenuItem newItem)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var menuItem = dbContext.Menu.Add(newItem).Entity;
            dbContext.SaveChanges();

            return menuItem;
        }

        public MenuItem UpdateMenuItem(int id, MenuItem newItem)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            if (!dbContext.Menu.Any(x => x.Id == id))
            {
                throw new NotFoundException($"Not found menu item with id = {id}");
            }

            newItem.Id = id;
            var menuItem = dbContext.Menu.Update(newItem).Entity;
            dbContext.SaveChanges();

            return menuItem;
        }

        public void DeleteMenuItem(int id)
        {
            using var dbContext = this.dbCxtFactory.CreateDbContext();
            var menuItem = dbContext.Menu.FirstOrDefault(x => x.Id == id);

            if (menuItem != null)
            {
				dbContext.Remove(menuItem);
				dbContext.SaveChanges();
			}
        }
    }
}
