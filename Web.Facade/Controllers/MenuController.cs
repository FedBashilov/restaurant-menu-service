// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Core.Models;
    using Menu.Service.Interfaces;
    using Menu.Service.Models.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Web.Facade.Responses;

    [Route("api/v1/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly ILogger<MenuController> logger;

        public MenuController(
            IMenuService menuService,
            IStringLocalizer<SharedResources> localizer,
            ILogger<MenuController> logger)
        {
            this.menuService = menuService;
            this.localizer = localizer;
            this.logger = logger;
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(List<MenuItemResponse>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetMenu(
            [FromQuery][Range(0, int.MaxValue)] int offset = 0,
            [FromQuery][Range(1, int.MaxValue)] int count = 100,
            [FromQuery] string? categories = default,
            [FromQuery] string? ids = default,
            [FromQuery] bool orderDesc = false,
            [FromQuery] bool onlyVisible = true)
        {
            try
            {
                var categoryIds = categories != default ? Array.ConvertAll(
                    categories.Split(','),
                    int.Parse) : default;

                var menuItemIds = ids != default ? Array.ConvertAll(
                    ids.Split(','),
                    int.Parse) : default;

                var menu = await this.menuService.GetMenu(
                    offset,
                    count,
                    categoryIds,
                    menuItemIds,
                    orderDesc,
                    onlyVisible);

                return this.Ok(menu);
            }
            catch (FormatException ex)
            {
                this.logger.LogError(ex, $"Can't get menu. {ex.Message}");
                return this.StatusCode(400, new ErrorResponse("Invalid request query parameter"));
            }
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(MenuItem))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetMenuItem([FromRoute] int id)
        {
            var menuItem = await this.menuService.GetMenuItem(id);

            return this.Ok(menuItem);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("")]
        [ProducesResponseType(201, Type = typeof(MenuItem))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemDTO menuItemDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            var menuItem = await this.menuService.CreateMenuItem(menuItemDto);

            return this.StatusCode(201, menuItem);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(MenuItem))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateMenuItem([FromRoute] int id, [FromBody] MenuItemDTO menuItemDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            var menuItem = await this.menuService.UpdateMenuItem(id, menuItemDto);

            return this.Ok(menuItem);
        }

        private bool IsInputModelValid([NotNullWhen(false)]out string? errorMessage)
        {
            if (!this.ModelState.IsValid)
            {
                errorMessage = this.ModelState
                    .SelectMany(state => state.Value!.Errors)
                    .Aggregate(string.Empty, (current, error) => current + (this.localizer[error.ErrorMessage].Value + ". "));

                return false;
            }

            errorMessage = null;

            return true;
        }
    }
}
