using AgeRanger.Domain.ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Domain.ServiceBus.EventHandler
{
    public abstract class VersionedEvent : IEvent, IOperator
    {
        public Guid EventId
        {
            get;
        } = Guid.NewGuid();

        public byte[] EventVersion { get; set; }

        public abstract Operator Operator { get; }

        public override string ToString()
        {
            return $"{EventId}|{Encoding.UTF8.GetString(EventVersion)}";
        }
    }
}
