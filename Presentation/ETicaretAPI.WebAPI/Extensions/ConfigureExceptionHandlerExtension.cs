using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;

namespace ETicaretAPI.WebAPI.Extensions
{
    public static class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionObject != null)
                    {
                        logger.LogError(exceptionObject.Error.Message);

                        await context.Response.WriteAsJsonAsync(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = exceptionObject.Error,
                            Title = "Internal Server Error!"
                        });
                    }
                });
            });
        }
    }
}
