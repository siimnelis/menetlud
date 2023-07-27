using Menetlus.External.Contracts.Models;

namespace Xtee.Connector.Extensions;

public static class MenetlejaExtensions
{
    public static TeavitusTeenus.Menetleja? Map(this Menetleja? menetleja)
    {
        if (menetleja == null)
            return null;
        
        return new TeavitusTeenus.Menetleja
        {
            Isikukood = menetleja.Isikukood,
            AsutuseTunnus = menetleja.AsutuseTunnus
        };
    }
}