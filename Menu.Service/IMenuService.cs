// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service
{
    using Infrastructure.Core.Models;
    using Menu.Service.Models.DTOs;

    public interface IMenuService
    {
        public Task<List<MenuItem>> GetMenu(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = true);

        public Task<MenuItem> GetMenuItem(int id);

        public Task<MenuItem> CreateMenuItem(MenuItemDTO menuItem);

        public Task<MenuItem> UpdateMenuItem(int id, MenuItemDTO menuItem);

        public Task DeleteMenuItem(int id);
    }
}
