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
                return this.NotFound($"Not found menuItem with id = {id} while executing GetMenuItem method");
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateMenuItem([FromBody] MenuItem newItem)
        {
            var menuItem = this.menuService.CreateMenuItem(newItem);
            return this.StatusCode(201, menuItem);
        }
    }
}
