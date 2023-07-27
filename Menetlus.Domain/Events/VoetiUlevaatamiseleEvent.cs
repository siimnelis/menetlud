namespace Menetlus.Domain.Events;

public record VoetiUlevaatamiseleEvent : Event
{
    public required Staatus Staatus { get; set; }
}