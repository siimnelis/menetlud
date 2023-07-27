namespace Menetlus.Domain.Events;

public record VoetiMenetlusseEvent : Event
{
    public required Staatus Staatus { get; set; }
}