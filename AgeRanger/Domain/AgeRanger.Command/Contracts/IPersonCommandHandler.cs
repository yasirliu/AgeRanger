using AgeRanger.Command.CommandValidaters;
using AgeRanger.Command.PersonCommand;
using AgeRanger.Domain.ServiceBus.Interfaces;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Command.Contracts
{
    [Intercept(typeof(CommandPropertyValidator))]
    public interface IPersonCommandHandler : 
        ICommandHandler<CreateNewPersonCommand>,
        ICommandHandler<ModifyExistingPersonCommand>
    {
    }
}
