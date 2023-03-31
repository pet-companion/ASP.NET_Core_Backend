using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using PetCareCore.ViewModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public class MyAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new AuthorizationMiddlewareResultHandler();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext httpContext,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(new APIResponse(false, "Unauthorized: Access is denied."));
            return;
        }
        if (authorizeResult.Forbidden)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await httpContext.Response.WriteAsJsonAsync(new APIResponse(false, "Permissions: You don't have permission to access this page."));
            return;
        }
        
        // Fall back to the default implementation.
        await defaultHandler.HandleAsync(next, httpContext, policy, authorizeResult);
    }
}