// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Database
{
    using Infrastructure.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public class MenuDatabaseContext : DbContext
    {
        public MenuDatabaseContext(DbContextOptions<MenuDatabaseContext> options)
            : base(options) => this.Database.EnsureCreated();

        public DbSet<MenuItem> Menu => this.Set<MenuItem>();
    }
}