namespace Menetlus.Domain.Events;

public abstract record Event
{
    public required int MenetlusId { get; set; }
}