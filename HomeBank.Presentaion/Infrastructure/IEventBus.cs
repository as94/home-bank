using System;

namespace HomeBank.Presentaion.Infrastructure
{
    public interface IEventBus
    {
        event EventBusHandler EventOccured;
        void Notify(EventType type, EventArgs args = null);
    }
}
