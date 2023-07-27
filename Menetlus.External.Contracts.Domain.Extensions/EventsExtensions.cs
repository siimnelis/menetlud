using Menetlus.External.Contracts.Events;
using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts.Domain.Extensions;

public static class EventsExtensions
{
    public static Event Map(this Menetlus.Domain.Events.Event @event)
    {
        if (@event is Menetlus.Domain.Events.MenetlusLoodudEvent menetlusLoodudEvent)
        {
            return new MenetlusLoodudEvent
            {
                MenetlusId = menetlusLoodudEvent.MenetlusId,
                Avaldaja = menetlusLoodudEvent.Avaldaja.Map(),
                Kusimus = menetlusLoodudEvent.Kusimus,
                Markus = menetlusLoodudEvent.Markus,
                Staatus = (Staatus) menetlusLoodudEvent.Staatus
            };
        } 
        
        if (@event is Menetlus.Domain.Events.VoetiUlevaatamiseleEvent voetiUlevaatamiseleEvent)
        {
            return new VoetiUlevaatamiseleEvent
            {
                MenetlusId = voetiUlevaatamiseleEvent.MenetlusId,
                Staatus = (Staatus) voetiUlevaatamiseleEvent.Staatus
            };
        }

        if (@event is Menetlus.Domain.Events.VoetiMenetlusseEvent voetiMenetlusseEvent)
        {
            return new VoetiMenetlusseEvent
            {
                MenetlusId = voetiMenetlusseEvent.MenetlusId,
                Staatus = (Staatus) voetiMenetlusseEvent.Staatus
            };
        }
        
        if (@event is Menetlus.Domain.Events.MenetlusLoppesEvent menetlusLoppesEvent)
        {
            return new MenetlusLoppesEvent
            {
                MenetlusId = menetlusLoppesEvent.MenetlusId,
                Staatus = (Staatus) menetlusLoppesEvent.Staatus,
                Vastus = (Vastus)menetlusLoppesEvent.Vastus
            };
        }
        
        throw new InvalidOperationException(@event.GetType().ToString());
    }
}