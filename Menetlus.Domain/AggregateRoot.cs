using System.Collections.ObjectModel;
using Menetlus.Domain.Events;

namespace Menetlus.Domain;

public abstract class AggregateRoot
{
    private List<Event> _events = new ();
    public IReadOnlyCollection<Event> Events => new ReadOnlyCollection<Event>(_events);

    protected void AddEvent(Event @event)
    {
        _events.Add(@event);
    }
}