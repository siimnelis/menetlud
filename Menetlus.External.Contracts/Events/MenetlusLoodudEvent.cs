using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts.Events;

public record MenetlusLoodudEvent : Event
{
    public required Avaldaja Avaldaja { get; set; }
    public required string Kusimus { get; set; }
    public required string Markus { get; set; }
    public required Staatus Staatus { get; set; }
}