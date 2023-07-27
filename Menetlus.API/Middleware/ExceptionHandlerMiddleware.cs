using Menetlus.Domain.Exceptions;

namespace Menetlus.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext httpContext, ILogger<ExceptionHandlerMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (EesnimiPuudubException)
        {
            await WriteErrorResponse("Eesnimi puudub.");
        }
        catch (EiOleMenetlusesException)
        {
            await WriteErrorResponse("Ei ole menetluses.");
        }
        catch (IsikukoodPuudubException)
        {
            await WriteErrorResponse("Isikukood puudub.");
        }
        catch (KusimusPuudubException)
        {
            await WriteErrorResponse("Küsimus puudub.");
        }
        catch (MenetlusEiOleOotelException)
        {
            await WriteErrorResponse("Menetlus ei ole ootel.");
        }
        catch (MenetlusEiOleUlevaatamiselException)
        {
            await WriteErrorResponse("Menetlus ei ole ülevaatamisel.");
        }
        catch (MenetlusOnLoppenudException)
        {
            await WriteErrorResponse("Menetlus on lõppenud.");
        }
        catch (MenetlustEiLeitudException)
        {
            await WriteErrorResponse("Menetlust ei leitud.");
        }
        catch (PerenimiPuudubException)
        {
            await WriteErrorResponse("Perenimi puudub.");
        }
        catch (VastusPuudubException)
        {
            await WriteErrorResponse("Vastus puudub.");
        }
        catch (ViganeVastusException)
        {
            await WriteErrorResponse("Vigane vastus.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception.");
            throw;
        }
        
        
        async Task WriteErrorResponse(string message, int statusCode = StatusCodes.Status400BadRequest)
        {
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.Headers.ContentType = "text/plain";

            await httpContext.Response.WriteAsync(message);
            logger.LogWarning(message);
        }
    }
}