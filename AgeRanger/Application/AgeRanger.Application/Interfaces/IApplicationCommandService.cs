using AgeRanger.Domain.ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Application.Interfaces
{
    public interface IApplicationCommandService<TCommand> : IApplicationService where TCommand : ICommand
    {
        void Apply(TCommand command);
        Task ApplyAsync(TCommand command);
    }
}
