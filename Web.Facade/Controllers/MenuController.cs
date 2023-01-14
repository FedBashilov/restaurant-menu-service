// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllDishes()
        {
            var menu = this.menuService.GetAllMenu();
            return this.Ok(JsonSerializer.Serialize(menu));
        }

        [HttpGet]
        [Route("{itemId}")]
        public IActionResult GetDishById([FromRoute] int dishId)
        {
            return this.Ok($"Get dish by id {dishId}");
        }
    }
}
