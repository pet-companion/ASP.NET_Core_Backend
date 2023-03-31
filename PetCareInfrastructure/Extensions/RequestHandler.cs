using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Extensions
{
    public static class RequestHandler
    {
        public static void HttpRequestHandler(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new APIResponse(false, "Unauthorized: Access is denied."));
                    return;
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(new APIResponse(false, "Permissions: You don't have permission to access this page."));
                    return;
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new APIResponse(false, "Page not found."));
                    return;
                }
            });
        }
    }
}
