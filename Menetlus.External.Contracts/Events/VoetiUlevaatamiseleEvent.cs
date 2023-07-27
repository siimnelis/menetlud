using Menetlus.External.Contracts.Models;

namespace Menetlus.External.Contracts.Events;

public record VoetiUlevaatamiseleEvent : Event
{
    public required Staatus Staatus { get; set; }
}