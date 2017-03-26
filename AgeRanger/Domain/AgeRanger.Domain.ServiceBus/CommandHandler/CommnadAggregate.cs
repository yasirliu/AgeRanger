using AgeRanger.Domain.ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Domain.ValueObjects;

namespace AgeRanger.Domain.ServiceBus.CommandHandler
{
    public abstract class CommnadAggregate : ICommand, IOperator
    {
        public Guid CommandId { get; } = Guid.NewGuid();

        public byte[] CommandVersion { get; set; }
        public abstract Operator Operator { get; }
    }
}
