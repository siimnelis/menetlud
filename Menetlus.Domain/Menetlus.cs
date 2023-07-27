using Menetlus.Domain.Events;
using Menetlus.Domain.Exceptions;

namespace Menetlus.Domain;

public class Menetlus : AggregateRoot
{
    public int Id { get; }
    public Avaldaja Avaldaja { get; }
    public string Kusimus { get; }
    public string Markus { get; }
    public Staatus Staatus { get; private set; }
    public Vastus Vastus { get; private set; }
    
    
    private Menetlus(int id, Avaldaja avaldaja, string kusimus, string? markus, Staatus staatus, Vastus vastus)
    {
        Id = id;
        Avaldaja = avaldaja;
        Kusimus = kusimus;
        Markus = markus!;
        Staatus = staatus;
        Vastus = vastus;
    }
    
    public Menetlus(IMenetlusIdGenerator menetlusIdGenerator, Avaldaja avaldaja, string kusimus, string? markus)
    {
        if (string.IsNullOrEmpty(kusimus))
            throw new KusimusPuudubException();
        
        Id = menetlusIdGenerator.GetNext();
        Avaldaja = avaldaja;
        Kusimus = kusimus;
        Markus = markus??"";
        Staatus = Staatus.Ootel;
        
        AddEvent(new MenetlusLoodudEvent
        {
            Avaldaja = Avaldaja,
            Kusimus = Kusimus,
            Markus = Markus,
            MenetlusId = Id,
            Staatus = Staatus
        });
    }

    public void VaataUle()
    {
        if (Staatus != Staatus.Ootel)
            throw new MenetlusEiOleOotelException();
        
        Staatus = Staatus.Ulevaatmisel;
        
        AddEvent(new VoetiUlevaatamiseleEvent
        {
            MenetlusId = Id,
            Staatus = Staatus
        });
    }

    public void VotaMenetlusse()
    {
        if (Staatus != Staatus.Ulevaatmisel)
            throw new MenetlusEiOleUlevaatamiselException();

        if (string.IsNullOrEmpty(Markus))
        {
            Staatus = Staatus.Menetluses;
            
            AddEvent(new VoetiMenetlusseEvent
            {
                MenetlusId = Id,
                Staatus = Staatus
            });
        }
        else
        {
            Staatus = Staatus.Tagasilukatud;
            
            AddEvent(new MenetlusLoppesEvent
            {
                MenetlusId = Id,
                Staatus = Staatus,
                Vastus = Vastus
            });
        }
    }

    public void Vasta(Vastus vastus)
    {
        if (Staatus == Staatus.Tagasilukatud || Staatus == Staatus.Lopetatud)
            throw new MenetlusOnLoppenudException();
        
        if (Staatus != Staatus.Menetluses)
            throw new EiOleMenetlusesException();
        
        if (vastus == Vastus.Puudub)
            throw new VastusPuudubException();

        if (!Enum.IsDefined(vastus))
            throw new ViganeVastusException();
        
        Vastus = vastus;
        Staatus = Staatus.Lopetatud;
        
        AddEvent(new MenetlusLoppesEvent
        {
            MenetlusId = Id,
            Staatus = Staatus,
            Vastus = Vastus
        });
    }
}