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
        public async Task<IActionResult> GetAllMenu()
        {
            var menu = await this.menuService.GetAllMenu();
            return this.Ok(JsonSerializer.Serialize(menu));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMenuItem([FromRoute] int id)
        {
            try
            {
                var menuItem = await this.menuService.GetMenuItem(id);
                return this.Ok(JsonSerializer.Serialize(menuItem));
            }
            catch (NotFoundException)
            {
                return this.NotFound($"Can't get menu item. Not found menu item with id = {id}");
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItem newItem)
        {
            var menuItem = await this.menuService.CreateMenuItem(newItem);
            return this.StatusCode(201, menuItem);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMenuItem([FromRoute] int id, [FromBody] MenuItem newItem)
        {
            try
            {
                var menuItem = await this.menuService.UpdateMenuItem(id, newItem);
                return this.Ok(menuItem);
            }
            catch (NotFoundException)
            {
                return this.NotFound($"Can't update menu item. Not found menu item with id = {id}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletetMenuItem([FromRoute] int id)
        {
            await this.menuService.DeleteMenuItem(id);
            return this.NoContent();
        }
    }
}
