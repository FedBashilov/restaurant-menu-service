﻿// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.Text.Json;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Web.Facade.Exceptions;
    using Web.Facade.Models;
    using Web.Facade.Services;

    [Route("api/v1/menu")]
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
            this.logger.LogInformation($"Starting to get all menu...");

            try
            {
                var menu = await this.menuService.GetAllMenu();

                this.logger.LogInformation($"All menu received successfully! Menu: {JsonSerializer.Serialize(menu)}. Sending the menu in response...");
                return this.Ok(menu);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't get menu. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't get menu. Unexpected error."));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMenuItem([FromRoute] int id)
        {
            this.logger.LogInformation($"Starting to get menu item with id = {id} ...");

            try
            {
                var menuItem = await this.menuService.GetMenuItem(id);

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
                return this.StatusCode(500, new ErrorResponse($"Can't get menu item. Unexpected error."));
            }
}

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItemDto menuItemDto)
        {
            if (menuItemDto.Name == null) { return this.BadRequest(new ErrorResponse("Name cannot be null.")); }
            if (menuItemDto.Price == null) { return this.BadRequest(new ErrorResponse("Price cannot be null.")); }

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
                return this.StatusCode(500, new ErrorResponse($"Can't create menu item. Unexpected error."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMenuItem([FromRoute] int id, [FromBody] MenuItemDto menuItemDto)
        {
            if (menuItemDto.Name == null) { return this.BadRequest(new ErrorResponse("Name cannot be null.")); }
            if (menuItemDto.Price == null) { return this.BadRequest(new ErrorResponse("Price cannot be null.")); }

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
                return this.StatusCode(500, new ErrorResponse($"Can't update menu item. Unexpected error."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMenuItem([FromRoute] int id)
        {
            this.logger.LogInformation($"Starting to delete menu item with id = {id} ...");

            try
            {
                await this.menuService.DeleteMenuItem(id);

                this.logger.LogInformation($"The menu item with id = {id} deleted successfully! Sending 204 response...");
                return this.NoContent();
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, $"Can't delete menu item. Unexpected error. Sending 500 response...");
                return this.StatusCode(500, new ErrorResponse($"Can't delete menu item. Unexpected error."));
            }
        }
    }
}
