using Menetlus.External.Contracts;

namespace Menetlus.Repository.EntityFrameworkCore;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Envelope Payload { get; set; }
    public string RoutingKey { get; set; } = String.Empty;
}