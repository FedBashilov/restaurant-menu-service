// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Interfaces
{
    using Infrastructure.Core.Models;
    using Menu.Service.Models.DTOs;

    public interface IMenuService
    {
        public Task<List<MenuItemResponse>> GetMenu(
            int offset = 0,
            int count = 100,
            IEnumerable<int>? categories = default,
            bool orderDesc = false,
            bool onlyVisible = true);

        public Task<MenuItemResponse> GetMenuItem(int id);

        public Task<MenuItemResponse> CreateMenuItem(MenuItemDTO menuItem);

        public Task<MenuItemResponse> UpdateMenuItem(int id, MenuItemDTO menuItem);
    }
}
