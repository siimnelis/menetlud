namespace Menetlus.Domain.Events;

public record MenetlusLoppesEvent : Event
{
    public required Staatus Staatus { get; set; }
    public required Vastus Vastus { get; set; }
}