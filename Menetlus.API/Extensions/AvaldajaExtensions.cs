using Menetlus.API.Models;

namespace Menetlus.API.Extensions;

public static class AvaldajaExtensions
{
    public static Domain.Avaldaja Map(this Avaldaja avaldaja)
    {
        return new Domain.Avaldaja(avaldaja.Eesnimi, avaldaja.Perenimi, avaldaja.Isikukood);
    }
    
    public static Avaldaja Map(this Domain.Avaldaja avaldaja)
    {
        if (avaldaja == null)
            throw new ArgumentNullException(nameof(avaldaja));
        
        return new Avaldaja
        {
            Eesnimi = avaldaja.Eesnimi,
            Perenimi = avaldaja.Perenimi,
            Isikukood = avaldaja.Isikukood
        };
    }
}