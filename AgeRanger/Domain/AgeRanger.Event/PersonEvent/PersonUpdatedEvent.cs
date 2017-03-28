using AgeRanger.Domain.ServiceBus;
using AgeRanger.Domain.ServiceBus.EventHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event.PersonEvent
{
    public sealed class PersonUpdatedEvent : VersionedEvent
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public override Operator Operator
        {
            get;
        } = Operator.Update;
    }
}
