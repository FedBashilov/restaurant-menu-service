// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Services
{
    using Web.Facade.Models;

    public interface IMenuService
    {
        public List<MenuItem> GetAllMenu();
    }
}
