// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Category.Service.Interfaces;
    using Category.Service.Models.DTOs;
    using Infrastructure.Auth.Constants;
    using Infrastructure.Core.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Shared.Exceptions;
    using Web.Facade.Responses;

    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IStringLocalizer<CategoryController> localizer;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(
            ICategoryService categoryService,
            IStringLocalizer<CategoryController> localizer,
            ILogger<CategoryController> logger)
        {
            this.categoryService = categoryService;
            this.localizer = localizer;
            this.logger = logger;
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("")]
        [ProducesResponseType(200, Type = typeof(List<Category>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetCategories(
            [FromQuery][Range(0, int.MaxValue)] int offset = 0,
            [FromQuery][Range(1, int.MaxValue)] int count = 100,
            [FromQuery] bool orderDesc = false,
            [FromQuery] bool onlyVisible = true)
        {
            try
            {
                var categories = await this.categoryService.GetCategories(offset, count, orderDesc, onlyVisible);
                return this.Ok(categories);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't get categories. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = $"{UserRoles.Client}, {UserRoles.Cook}, {UserRoles.Admin}")]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            try
            {
                var category = await this.categoryService.GetCategory(id);
                return this.Ok(category);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't get category. Not found category with id = {id}.");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't get category. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("")]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            try
            {
                var category = await this.categoryService.CreateCategory(categoryDto);

                return this.StatusCode(201, category);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't create category. {ex.Message}");
                return this.StatusCode(500, new ErrorResponse(this.localizer["Unexpected error"].Value));
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryDTO categoryDto)
        {
            if (!this.IsInputModelValid(out var message))
            {
                return this.StatusCode(400, new ErrorResponse(message));
            }

            try
            {
                var category = await this.categoryService.UpdateCategory(id, categoryDto);
                return this.Ok(category);
            }
            catch (NotFoundException ex)
            {
                this.logger.LogWarning(ex, $"Can't update category. Not found category with id = {id}.");
                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Can't update category. {ex.Message}");
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
