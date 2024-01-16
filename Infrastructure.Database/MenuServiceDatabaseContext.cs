// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Database
{
    using Infrastructure.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public class MenuServiceDatabaseContext : DbContext
    {
        public MenuServiceDatabaseContext(DbContextOptions<MenuServiceDatabaseContext> options)
            : base(options) => this.Database.EnsureCreated();

        public DbSet<MenuItem> Menu => this.Set<MenuItem>();

        public DbSet<Category> Categories => this.Set<Category>();

        public DbSet<MenuItemCategory> MenuItemCategories => this.Set<MenuItemCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItemCategory>()
                .HasKey(nameof(MenuItemCategory.MenuItemId), nameof(MenuItemCategory.CategoryId));

            base.OnModelCreating(modelBuilder);
        }
    }
}