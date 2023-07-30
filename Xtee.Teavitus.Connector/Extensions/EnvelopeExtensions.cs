using Menetlus.External.Contracts;
using Menetlus.External.Contracts.Events;
using Xtee.Teavitus.Connector.TeavitusTeenus;

namespace Xtee.Teavitus.Connector.Extensions;

public static class EnvelopeExtensions
{
    public static string GetUserId(this Envelope envelope)
    {
        if (envelope.Event is MenetlusLoodudEvent menetlusLoodudEvent)
        {
            return menetlusLoodudEvent.Avaldaja.Isikukood;
        }

        return envelope.Menetleja?.Isikukood ?? "";
    }
    
    public static TeavitusTeenus.Teavitus Map(this Envelope envelope, string id)
    {
        if (envelope.Event is MenetlusLoodudEvent menetlusLoodudEvent)
        {
            return new MenetlusLoodud
            {
                MenetlusId = menetlusLoodudEvent.MenetlusId,
                Avaldaja = menetlusLoodudEvent.Avaldaja.Map(),
                Kusimus = menetlusLoodudEvent.Kusimus,
                Markus = menetlusLoodudEvent.Markus,
                Staatus = menetlusLoodudEvent.Staatus.Map(),
                Menetleja = envelope.Menetleja.Map(),
                TeavitusId = id
            };
        } 
        
        if (envelope.Event is VoetiUlevaatamiseleEvent voetiUlevaatamiseleEvent)
        {
            return new VoetiUlevaatamisele
            {
                MenetlusId = voetiUlevaatamiseleEvent.MenetlusId,
                Staatus = voetiUlevaatamiseleEvent.Staatus.Map(),
                Menetleja = envelope.Menetleja.Map(),
                TeavitusId = id
            };
        }

        if (envelope.Event is VoetiMenetlusseEvent voetiMenetlusseEvent)
        {
            return new VoetiMenetlusse
            {
                MenetlusId = voetiMenetlusseEvent.MenetlusId,
                Staatus = voetiMenetlusseEvent.Staatus.Map(),
                Menetleja = envelope.Menetleja.Map(),
                TeavitusId = id
            };
        }
        
        if (envelope.Event is MenetlusLoppesEvent menetlusLoppesEvent)
        {
            return new MenetlusLoppes
            {
                MenetlusId = menetlusLoppesEvent.MenetlusId,
                Staatus = menetlusLoppesEvent.Staatus.Map(),
                Vastus = menetlusLoppesEvent.Vastus.Map(),
                Menetleja = envelope.Menetleja.Map(),
                TeavitusId = id
            };
        }
        
        throw new InvalidOperationException(envelope.Event.GetType().ToString());
    }
}