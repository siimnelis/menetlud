using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts.Domain.Extensions;

public static class AvaldajaExtensions
{
    public static Avaldaja Map(this Menetlus.Domain.Avaldaja avaldaja)
    {
        return new Avaldaja
        {
            Eesnimi = avaldaja.Eesnimi,
            Perenimi = avaldaja.Perenimi,
            Isikukood = avaldaja.Isikukood
        };
    }
}