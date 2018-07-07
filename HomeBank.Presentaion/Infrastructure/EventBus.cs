using System;

namespace HomeBank.Presentaion.Infrastructure
{
    public sealed class EventBus : IEventBus
    {
        public event EventBusHandler EventOccured;

        public void Notify(EventType type, EventArgs args = null)
        {
            EventOccured?.Invoke(type, args);
        }
    }
}
