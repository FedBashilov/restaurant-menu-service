// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Middlewares
{
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Category.Service.Exceptions;
    using Menu.Service.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Web.Facade.Responses;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            IStringLocalizer<SharedResources> localizer,
            ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.localizer = localizer;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    exception,
                    $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
                var response = context.Response;

                var (status, message) = this.GetResponse(exception);
                response.StatusCode = (int)status;
                await response.WriteAsJsonAsync(new ErrorResponse(message));
            }
        }

        private (HttpStatusCode code, string msg) GetResponse(Exception ex)
        {
            HttpStatusCode code;
            var msg = string.Empty;
            switch (ex)
            {
                case CategoryNotFoundException or MenuItemNotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case UpdateCategoryFailedException or UpdateMenuItemFailedException:
                    msg = this.localizer["Unexpected server error"].Value;
                    code = HttpStatusCode.InternalServerError;
                    break;
                default:
                    msg = this.localizer["Unexpected server error"].Value;
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            return (code, msg);
        }
    }
}