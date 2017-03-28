using AgeRanger.Domain.ServiceBus.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event
{
    public class UnKnownErrorEvent : ExceptionEvent
    {
        public UnKnownErrorEvent(string message) : base(message)
        {
        }
    }
}
