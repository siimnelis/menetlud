using Menetlus.API.Models;

namespace Menetlus.API.Extensions;

public static class MenetlusExtensions
{
    public static Models.Menetlus Map(this Domain.Menetlus menetlus)
    {
        if (menetlus == null)
            throw new ArgumentNullException(nameof(menetlus));

        if (menetlus.Avaldaja == null)
            throw new ArgumentNullException(nameof(menetlus.Avaldaja));
        
        return new Models.Menetlus
        {
            Id = menetlus.Id,
            Avaldaja = menetlus.Avaldaja.Map(),
            Staatus = (Staatus)menetlus.Staatus,
            Vastus = (Vastus)menetlus.Vastus
        };
    }
}