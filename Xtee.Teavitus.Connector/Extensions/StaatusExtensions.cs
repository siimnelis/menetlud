using Staatus = Menetlus.External.Contracts.Models.Staatus;

namespace Xtee.Teavitus.Connector.Extensions;

public static class StaatusExtensions
{
    public static TeavitusTeenus.Staatus Map(this Staatus staatus)
    {
        switch (staatus)
        {
            case Staatus.Ootel:
                return TeavitusTeenus.Staatus.Ootel;
            case Staatus.Ulevaatmisel:
                return TeavitusTeenus.Staatus.Ulevaatamisel;
            case Staatus.Menetluses:
                return TeavitusTeenus.Staatus.Menetluses;
            case Staatus.Tagasilukatud:
                return TeavitusTeenus.Staatus.TagasiLukatud;
            case Staatus.Lopetatud:
                return TeavitusTeenus.Staatus.Lopetatud;
            default: throw new InvalidOperationException(staatus.GetType().ToString());
        }
    }
}