// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Auth.Services;
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
            [FromQuery] int offset = 0,
            [FromQuery] int count = 100,
            [FromQuery] bool orderDesc = false)
        {
            this.logger.LogInformation($"Starting to get all menu...");

            try
            {
                var jwtEncoded = await this.HttpContext.GetTokenAsync("access_token");
                var userRole = JwtService.GetClaimValue(jwtEncoded, ClaimTypes.Role);
                var onlyVisible = userRole == UserRoles.Client;

                var menu = await this.menuService.GetMenu(offset, count, orderDesc, onlyVisible);

                this.logger.LogInformation($"All menu received successfully! Menu: {JsonSerializer.Serialize(menu)}. Sending the menu in response...");
                return this.Ok(menu);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't get menu. Unexpected error. Sending 500 response...");
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
            this.logger.LogInformation($"Starting to get menu item with id = {id} ...");

            try
            {
                var jwtEncoded = await this.HttpContext.GetTokenAsync("access_token");
                var userRole = JwtService.GetClaimValue(jwtEncoded, ClaimTypes.Role);
                var onlyVisible = userRole == UserRoles.Client;

                var menuItem = await this.menuService.GetMenuItem(id, onlyVisible);

                this.logger.LogInformation($"The menu item with id = {id} received successfully! Menu item: {JsonSerializer.Serialize(menuItem)}. Sending the menu item in response...");
                return this.Ok(menuItem);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't get menu item. Not found menu item with id = {id}. Sending 404 response...");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't get menu item. Unexpected error. Sending 500 response...");
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

            this.logger.LogInformation($"Starting to create menu item: {JsonSerializer.Serialize(menuItemDto)} ...");

            try
            {
                var menuItem = await this.menuService.CreateMenuItem(menuItemDto);

                this.logger.LogInformation($"The menu item created successfully! Menu item: {JsonSerializer.Serialize(menuItem)}. Sending the menu item in response...");
                return this.StatusCode(201, menuItem);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't create menu item. Unexpected error. Sending 500 response...");
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

            this.logger.LogInformation($"Starting to update menu item with id = {id}. Menu item = {JsonSerializer.Serialize(menuItemDto)} ...");

            try
            {
                var menuItem = await this.menuService.UpdateMenuItem(id, menuItemDto);

                this.logger.LogInformation($"The menu item with id = {id} updated successfully! Menu item: {JsonSerializer.Serialize(menuItem)}. Sending the menu item in response...");
                return this.Ok(menuItem);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update menu item. Not found menu item with id = {id}. Sending 404 response...");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't update menu item. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        private bool IsInputModelValid(out string? errorMessage)
        {
            if (!this.ModelState.IsValid)
            {
                errorMessage = this.ModelState
                    .SelectMany(state => state.Value.Errors)
                    .Aggregate(string.Empty, (current, error) => current + (error.ErrorMessage + ". "));

                return false;
            }

            errorMessage = null;

            return true;
        }
    }
}
