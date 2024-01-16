// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Category.Service
{
    using System.Linq;
    using Category.Service.Interfaces;
    using Category.Service.Models.DTOs;
    using Infrastructure.Core.Models;
    using Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using Shared.Exceptions;

    public class CategoryService : ICategoryService
    {
        private readonly IDbContextFactory<MenuServiceDatabaseContext> dbCxtFactory;

        public CategoryService(
            IDbContextFactory<MenuServiceDatabaseContext> dbCxtFactory)
        {
            this.dbCxtFactory = dbCxtFactory;
        }

        public async Task<List<Category>> GetCategories(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

            var selectQuery = onlyVisible ?
                dbContext.Categories.Where(x => x.Visible == true) :
                dbContext.Categories;

            var orderQuery = orderDesc ?
                selectQuery.OrderByDescending(x => x.Id) :
                selectQuery.OrderBy(x => x.Id);

            var pageQuery = orderQuery.Skip(offset).Take(count);

            var categories = await pageQuery.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategory(int id)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

            var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new NotFoundException($"Not found category with id = {id} while executing GetCategory method");
            }

            return category;
        }

        public async Task<Category> CreateCategory(CategoryDTO newItemDto)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

            var newItem = new Category()
            {
                Name = newItemDto.Name,
                Visible = newItemDto.Visible,
            };

            var category = (await dbContext.Categories.AddAsync(newItem)).Entity;

            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategory(int id, CategoryDTO newItemDto)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();
            if (!await dbContext.Categories.AnyAsync(x => x.Id == id))
            {
                throw new NotFoundException();
            }

            var newItem = new Category()
            {
                Id = id,
                Name = newItemDto.Name,
                Visible = newItemDto.Visible,
            };

            var category = dbContext.Categories.Update(newItem).Entity;
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategory(int id)
        {
            await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();
            var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category != null)
            {
                dbContext.Categories.Remove(category);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
