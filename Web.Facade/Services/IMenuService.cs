// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;

    public interface IMenuService
    {
        public Task<List<MenuItem>> GetMenu(int offset = 0, int count = 100, bool orderDesc = false, bool onlyVisible = false);

        public Task<MenuItem> GetMenuItem(int id, bool onlyVisible = false);

        public Task<MenuItem> CreateMenuItem(MenuItemDto menuItem);

        public Task<MenuItem> UpdateMenuItem(int id, MenuItemDto menuItem);

        public Task DeleteMenuItem(int id);
    }
}
