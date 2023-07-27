using System.Security.Claims;

namespace Menetlus.API.Authentication;

public class MenetlejaPrincipal : ClaimsPrincipal
{
    public MenetlejaPrincipal(MenetlejaIdentity menetlejaIdentity) : base(menetlejaIdentity)
    {

    }
}