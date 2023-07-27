using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts.Events;

public record MenetlusLoppesEvent : Event
{ 
    public required Staatus Staatus { get; set; }
    public required Vastus Vastus { get; set; }
}