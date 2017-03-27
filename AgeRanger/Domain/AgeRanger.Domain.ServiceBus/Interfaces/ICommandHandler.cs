using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Domain.ServiceBus.Interfaces
{
    public interface ICommandHandler<in TCommand> : IDisposable where TCommand : ICommand
    {
        void Handle(TCommand command);
        Task HandleAsync(TCommand command);
    }
}
