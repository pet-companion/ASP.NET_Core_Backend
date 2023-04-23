using Microsoft.AspNetCore.Http;
using PetCareCore.Enum;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Middleware
{
    public class RequestHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(
                    new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.InternalServerError, "Something went wrong.", ex.Message));
                return;
            }
            switch (context.Response.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(
                        new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.Unauthorized, "Unauthorized: Access is denied."));
                    return;
                case (int)HttpStatusCode.Forbidden:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(
                        new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.Forbidden, "Permissions: You don't have permission to access this page."));
                    return;
                case (int)HttpStatusCode.NotFound:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(
                        new APIResponse(StatusMessageEnum.Failed.ToDisplayName(), (int)HttpStatusCode.NotFound, "Page not found."));
                    return;
            }
        }
    }
}

