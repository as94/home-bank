using System;

namespace HomeBank.Presentaion.Infrastructure
{
    public delegate void EventBusHandler(EventType type, EventArgs args = null);
}
