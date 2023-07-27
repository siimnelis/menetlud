
using Menetlus.External.Contracts.Events;
using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts;

public class Envelope
{
    public required Menetleja? Menetleja { get; set; }
    public required Event Event { get; set; }
}