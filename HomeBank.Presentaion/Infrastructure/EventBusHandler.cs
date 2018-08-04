using System;

namespace HomeBank.Presentation.Infrastructure
{
    public delegate void EventBusHandler(EventType type, EventArgs args = null);
}
