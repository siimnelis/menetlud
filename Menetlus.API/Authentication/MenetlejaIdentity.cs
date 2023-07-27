using System.Security.Claims;

namespace Menetlus.API.Authentication;

public class MenetlejaIdentity : ClaimsIdentity
{
    public string Isikukood { get; }
    public string AsutuseTunnus { get; }
    
    public MenetlejaIdentity(string isikukood, string asutuseTunnus):base("Basic")
    {
        Isikukood = isikukood;
        AsutuseTunnus = asutuseTunnus;
    }
}