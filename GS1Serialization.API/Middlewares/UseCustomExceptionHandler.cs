using GS1Serialization.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using GS1Serialization.Application.DTOs;

namespace GS1Serialization.API.Middlewares;

public static class UseCustomExceptionHandler
{
   public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                var serviceProvider = context.RequestServices;
                var logger = serviceProvider.GetService<ILogger<Program>>();
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;
                context.Response.ContentType = "application/json";

                int statusCode;

                switch (exception)
                {
                    case BusinessException businessEx:
                        statusCode = 400;
                        logger?.LogWarning("İş kuralı hatası: {Message}", businessEx.Message);
                        break;

                    case NotFoundException notFoundEx:
                        statusCode = 404;
                        logger?.LogWarning("Veri bulunamadı: {Message}", notFoundEx.Message);
                        break;

                    default:
                        statusCode = 500;
                        logger?.LogError(exception, "Kritik Sunucu Hatası: {Message}", exception.Message);
                        break;
                }
                context.Response.StatusCode = statusCode;
                var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });
    }
}