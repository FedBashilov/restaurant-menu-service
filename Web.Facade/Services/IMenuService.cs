// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;

    public interface IMenuService
    {
        public Task<List<MenuItem>> GetAllMenu(bool onlyVisible);

        public Task<MenuItem> GetMenuItem(int id, bool onlyVisible);

        public Task<MenuItem> CreateMenuItem(MenuItemDto menuItem);

        public Task<MenuItem> UpdateMenuItem(int id, MenuItemDto menuItem);

        public Task DeleteMenuItem(int id);
    }
}
