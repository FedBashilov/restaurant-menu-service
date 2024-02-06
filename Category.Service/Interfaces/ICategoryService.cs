// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Category.Service.Interfaces
{
    using Category.Service.Models.DTOs;
    using Infrastructure.Core.Models;

    public interface ICategoryService
    {
        public Task<List<Category>> GetCategories(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true);

        public Task<Category> GetCategory(int id);

        public Task<Category> CreateCategory(CategoryDTO categoryItem);

        public Task<Category> UpdateCategory(int id, CategoryDTO categoryItem);
    }
}
