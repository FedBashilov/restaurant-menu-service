// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Middlewares
{
    using System.Net;
    using System.Threading.Tasks;
    using Infrastructure.Core.Exceptions;
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

                if (response.StatusCode != 404)
                {
                    await response.WriteAsJsonAsync(new ErrorResponse(message));
                }
            }
        }

        private (HttpStatusCode code, string msg) GetResponse(Exception ex)
        {
            HttpStatusCode code;
            var msg = string.Empty;
            switch (ex)
            {
                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    msg = this.localizer[ex.Message].Value;
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case InternalServerErrorException:
                    code = HttpStatusCode.InternalServerError;
                    msg = this.localizer[ex.Message].Value;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    msg = this.localizer["Unexpected server error"].Value;
                    break;
            }

            return (code, msg);
        }
    }
}