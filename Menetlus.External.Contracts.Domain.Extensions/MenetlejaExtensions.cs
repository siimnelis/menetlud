using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts.Domain.Extensions;

public static class MenetlejaExtensions
{
    public static Menetleja? Map(this Menetlus.Domain.Menetleja? menetleja)
    {
        if (menetleja == null)
            return null;
        
        return new Menetleja
        {
            Isikukood = menetleja.Isikukood,
            AsutuseTunnus = menetleja.AsutuseTunnus
        };
    }
}