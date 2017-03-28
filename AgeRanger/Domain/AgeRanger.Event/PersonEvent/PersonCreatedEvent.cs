using AgeRanger.Domain.ServiceBus;
using AgeRanger.Domain.ServiceBus.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event.PersonEvent
{
    public sealed class PersonCreatedEvent : VersionedEvent
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public override Operator Operator
        {
            get;
        } = Operator.Add;
    }
}
