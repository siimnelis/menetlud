using Menetlus.API.Authentication;
using Menetlus.Domain;

namespace Menetlus.API.Extensions;

public static class MenetlejaContextExtensions
{
    public static void AddMenetlejaContext(this IServiceCollection services)
    {
        services.AddScoped(serviceProvider =>
        {
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>()!;
            var httpContext = httpContextAccessor.HttpContext;
            
            if (httpContext != null && httpContext.User.Identity is MenetlejaIdentity {IsAuthenticated: true} menetlejaIdentity)
            {
                return new MenetlejaContext
                {
                    Menetleja = new Menetleja
                    {
                        Isikukood = menetlejaIdentity.Isikukood,
                        AsutuseTunnus = menetlejaIdentity.AsutuseTunnus
                    }
                };
            }

            return new MenetlejaContext
            {
                Menetleja = null
            };
        });
    }
}