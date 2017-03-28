using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Domain.ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event.Contracts
{
    public interface INegativeEventsHandler : IEventHandler<ExceptionEvent>
    {
    }
}
