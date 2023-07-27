using Menetlus.External.Contracts.Models;

namespace Xtee.Connector.Extensions;

public static class AvaldajaExtensions
{
    public static TeavitusTeenus.Avaldaja Map(this Avaldaja avaldaja)
    {
        return new TeavitusTeenus.Avaldaja
        {
            Eesnimi = avaldaja.Eesnimi,
            Perenimi = avaldaja.Perenimi,
            Isikukood = avaldaja.Isikukood
        };
    }
}