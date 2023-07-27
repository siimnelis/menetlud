using Menetlus.Domain.Exceptions;

namespace Menetlus.Domain;

public record Avaldaja
{
    public string Eesnimi { get; }
    public string Perenimi { get; }
    public string Isikukood { get; }

    public Avaldaja(string eesnimi, string perenimi, string isikukood)
    {
        if (string.IsNullOrEmpty(eesnimi))
            throw new EesnimiPuudubException();
        
        if (string.IsNullOrEmpty(perenimi))
            throw new PerenimiPuudubException();
        
        if (string.IsNullOrEmpty(isikukood))
            throw new IsikukoodPuudubException();
        
        Eesnimi = eesnimi;
        Perenimi = perenimi;
        Isikukood = isikukood;
    }
}