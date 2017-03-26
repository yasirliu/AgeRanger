using AgeRanger.Domain.ServiceBus.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event.PersonEvent
{
    public class PersonNotUpdatedEvent : ExceptionEvent
    {
        public PersonNotUpdatedEvent(string errorMessage) : base(errorMessage)
        {
        }
    }
}
