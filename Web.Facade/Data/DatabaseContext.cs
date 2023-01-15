// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Data
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Models;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) => Database.EnsureCreated();

        public DbSet<MenuItem> Menu => Set<MenuItem>();
    }
}