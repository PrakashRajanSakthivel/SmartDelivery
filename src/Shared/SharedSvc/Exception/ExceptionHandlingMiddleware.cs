using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common;
using SharedSvc.Response;
using System.Net;
using System.Text.Json;

namespace SharedSvc.Exception
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OrderBusinessValidationException businessValidationEx)
            {
                _logger.LogWarning("Business validation failed: {Errors}",
                    string.Join(", ", businessValidationEx.ValidationResult.Errors));

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = businessValidationEx.ValidationResult.Errors.Select(e => new ApiError(
                    e,
                    null,
                    "BUSINESS_VALIDATION")).ToList();

                var response = SharedSvc.Response.ApiResponse<object>.ValidationError(errors);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (ValidationException validationEx)
            {
                _logger.LogWarning("Validation failed: {Errors}",
                    string.Join(", ", validationEx.Errors.Select(e => e.ErrorMessage)));

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = validationEx.Errors.Select(e => new ApiError(
                    e.ErrorMessage,
                    e.PropertyName,
                    e.ErrorCode)).ToList();

                var response = SharedSvc.Response.ApiResponse<object>.ValidationError(errors);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (KeyNotFoundException notFoundEx)
            {
                _logger.LogWarning("Resource not found: {Message}", notFoundEx.Message);

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var response = SharedSvc.Response.ApiResponse<object>.Error("Resource not found");

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (UnauthorizedAccessException unauthorizedEx)
            {
                _logger.LogWarning("Unauthorized access: {Message}", unauthorizedEx.Message);

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var response = SharedSvc.Response.ApiResponse<object>.Error("Unauthorized");

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Database connection error: {Message}", sqlEx.Message);

                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.ContentType = "application/json";

                var message = _environment.IsDevelopment()
                    ? $"Database error: {sqlEx.Message}"
                    : "Database service is temporarily unavailable";

                var response = SharedSvc.Response.ApiResponse<object>.Error(message);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error: {Message}", dbEx.Message);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var message = _environment.IsDevelopment()
                    ? $"Database update error: {dbEx.Message}"
                    : "Unable to update the requested resource";

                var response = SharedSvc.Response.ApiResponse<object>.Error(message);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught!");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // Show detailed error in development, generic in production
                var message = _environment.IsDevelopment()
                    ? $"An error occurred: {ex.Message}"
                    : "An internal server error occurred";

                var response = SharedSvc.Response.ApiResponse<object>.Error(message);

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}