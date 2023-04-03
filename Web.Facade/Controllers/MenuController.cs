// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Core.Models;
    using Menu.Service;
    using Menu.Service.Exceptions;
    using Menu.Service.Models.DTOs;
    using Menu.Service.Models.Responses;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Route("api/v1/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        private readonly IStringLocalizer<MenuController> localizer;
        private readonly ILogger<MenuController> logger;

        public MenuController(
            IMenuService menuService,
            IStringLocalizer<MenuController> localizer,
            ILogger<MenuController> logger)
        {
            this.menuService = menuService;
            this.localizer = localizer;
            this.logger = logger;
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(List<MenuItem>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetMenu(
            [FromQuery][Range(0, int.MaxValue)] int offset = 0,
            [FromQuery][Range(1, int.MaxValue)] int count = 100,
            [FromQuery] bool orderDesc = false,
            [FromQuery] bool onlyVisible = true)
        {
            try
            {
                var menu = await this.menuService.GetMenu(offset, count, orderDesc, onlyVisible);
                return this.Ok(menu);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't get menu. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(MenuItem))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetMenuItem([FromRoute] int id)
        {
            try
            {
                var menuItem = await this.menuService.GetMenuItem(id);
                return this.Ok(menuItem);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't get menu item. Not found menu item with id = {id}.");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't get menu item. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
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

            try
            {
                var menuItem = await this.menuService.CreateMenuItem(menuItemDto);
                return this.StatusCode(201, menuItem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't create menu item. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
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

            try
            {
                var menuItem = await this.menuService.UpdateMenuItem(id, menuItemDto);
                return this.Ok(menuItem);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update menu item. Not found menu item with id = {id}.");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't update menu item. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        private bool IsInputModelValid([NotNullWhen(false)]out string? errorMessage)
        {
            if (!this.ModelState.IsValid)
            {
                errorMessage = this.ModelState
                    .SelectMany(state => state.Value!.Errors)
                    .Aggregate(string.Empty, (current, error) => current + (error.ErrorMessage + ". "));

                return false;
            }

            errorMessage = null;

            return true;
        }
    }
}
