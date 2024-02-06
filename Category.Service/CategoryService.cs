// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Category.Service
{
    using System.Linq;
    using Category.Service.Exceptions;
    using Category.Service.Interfaces;
    using Category.Service.Models.DTOs;
    using Infrastructure.Core.Exceptions;
    using Infrastructure.Core.Models;
    using Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;

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
            try
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
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to get Categories", ex);
            }
        }

        public async Task<Category> GetCategory(int id)
        {
            try
            {
                await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

                var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    throw new CategoryNotFoundException($"Not found category with id = {id} while executing GetCategory method");
                }

                return category;
            }
            catch (CategoryNotFoundException ex)
            {
                throw new NotFoundException("Category Not found", ex);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to get Category", ex);
            }
        }

        public async Task<Category> CreateCategory(CategoryDTO newItemDto)
        {
            try
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
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to create Category", ex);
            }
        }

        public async Task<Category> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            try
            {
                await using var dbContext = await this.dbCxtFactory.CreateDbContextAsync();

                var category = await dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == default)
                {
                    throw new CategoryNotFoundException();
                }

                category.Name = categoryDto.Name ?? category.Name;
                category.Visible = categoryDto.Visible;

                dbContext.Categories.Update(category);

                await dbContext.SaveChangesAsync();

                return category;
            }
            catch (CategoryNotFoundException ex)
            {
                throw new NotFoundException("Category Not found", ex);
            }
            catch (Exception ex)
            {
                throw new InternalServerErrorException("Failed to update Category", ex);
            }
        }
    }
}
