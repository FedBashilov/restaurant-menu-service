// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Data
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Models;

    public class MenuDatabaseContext : DbContext
    {
        public MenuDatabaseContext(DbContextOptions<MenuDatabaseContext> options)
            : base(options) => this.Database.EnsureCreated();

        public DbSet<MenuItem> Menu => this.Set<MenuItem>();
    }
}