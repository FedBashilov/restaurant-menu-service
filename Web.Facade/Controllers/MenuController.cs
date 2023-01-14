// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;
    using Web.Facade.Services;

    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        private readonly ILogger<MenuController> logger;

        public MenuController(IMenuService menuService, ILogger<MenuController> logger)
        {
            this.menuService = menuService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllMenu()
        {
            var menu = this.menuService.GetAllMenu();
            return this.Ok(JsonSerializer.Serialize(menu));
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetMenuItem([FromRoute] int id)
        {
            try
            {
                var menuItem = this.menuService.GetMenuItem(id);
                return this.Ok(JsonSerializer.Serialize(menuItem));
            }
            catch (NotFoundException)
            {
                return this.NotFound($"Can't get menu item. Not found menu item with id = {id}");
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMenuItem([FromBody] MenuItem newItem)
        {
            var menuItem = this.menuService.CreateMenuItem(newItem);
            return this.StatusCode(201, menuItem);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateMenuItem([FromRoute] int id, [FromBody] MenuItem newItem)
        {
            try
            {
                var menuItem = this.menuService.UpdateMenuItem(id, newItem);
                return this.Ok(menuItem);
            }
            catch (NotFoundException)
            {
                return this.NotFound($"Can't update menu item. Not found menu item with id = {id}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeletetMenuItem([FromRoute] int id)
        {
            this.menuService.DeleteMenuItem(id);
            return this.NoContent();
        }
    }
}
