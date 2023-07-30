using Menetlus.External.Contracts.Models;

namespace Xtee.Teavitus.Connector.Extensions;

public static class VastusExtensions
{
    public static TeavitusTeenus.Vastus Map(this Vastus vastus)
    {
        switch (vastus)
        {
            case Vastus.Puudub:
                return TeavitusTeenus.Vastus.Puudub;
            case Vastus.Jah:
                return TeavitusTeenus.Vastus.Jah;
            case Vastus.Ei:
                return TeavitusTeenus.Vastus.Ei;
            default: throw new InvalidOperationException(vastus.GetType().ToString());
        }
    }
}