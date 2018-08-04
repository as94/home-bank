using System;

namespace HomeBank.Presentation.Infrastructure
{
    public interface IEventBus
    {
        event EventBusHandler EventOccured;
        void Notify(EventType type, EventArgs args = null);
    }
}
