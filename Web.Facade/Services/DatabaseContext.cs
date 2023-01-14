// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Models;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) => this.Database.EnsureCreated();

        public DbSet<MenuItem> Menu => this.Set<MenuItem>();
    }
}