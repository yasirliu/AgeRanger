using AgeRanger.Domain.ServiceBus.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event.PersonEvent
{
    public class PersonNotCreatedEvent : ExceptionEvent
    {
        public PersonNotCreatedEvent(string errorMessage) : base(errorMessage)
        {
        }
    }
}
