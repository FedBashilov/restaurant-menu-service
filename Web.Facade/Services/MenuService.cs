// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Microsoft.EntityFrameworkCore;
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
            return dbContext.Menu.ToList<MenuItem>();
        }
    }
}
