// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;

    public interface IMenuService
    {
		public List<MenuItem> GetAllMenu();

        public MenuItem GetMenuItem(int id);

        public MenuItem CreateMenuItem(MenuItem menuItem);

        public MenuItem UpdateMenuItem(int id, MenuItem menuItem);

        public void DeleteMenuItem(int id);
    }
}
