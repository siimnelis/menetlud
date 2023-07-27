using Menetlus.Repository.EntityFrameworkCore;

namespace Menetlus.API.Middleware;

public class UnitOfWorkMiddleware
{
    private RequestDelegate _next;

    public UnitOfWorkMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext httpContext, MenetlusContext menetlusContext)
    {
        if (httpContext.Request.Method == HttpMethod.Get.Method)
            await _next(httpContext);
        else
        {
            try
            {
                await menetlusContext.Database.BeginTransactionAsync();
                await _next(httpContext);
                await menetlusContext.SaveChangesAsync();
                await menetlusContext.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await menetlusContext.Database.RollbackTransactionAsync();
                throw;
            }
        }
    }
}