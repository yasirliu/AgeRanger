using AgeRanger.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Command.PersonCommand;
using AgeRanger.Command.Contracts;
using AgeRanger.Application.Interfaces;

namespace AgeRanger.Application.CommandServices
{
    public class PersonCommandService : IPersonCommandServiceContract
    {
        IPersonCommandHandler _handler;
        public PersonCommandService(IPersonCommandHandler handler)
        {
            _handler = handler;
        }


        public void Apply(CreateNewPersonCommand command)
        {
            _handler.Handle(command);
        }

        public async Task ApplyAsync(CreateNewPersonCommand command)
        {
            await _handler.HandleAsync(command);
        }

        public void Apply(ModifyExistingPersonCommand command)
        {
            _handler.Handle(command);
        }

        public async Task ApplyAsync(ModifyExistingPersonCommand command)
        {
            await _handler.HandleAsync(command);
        }
    }
}
