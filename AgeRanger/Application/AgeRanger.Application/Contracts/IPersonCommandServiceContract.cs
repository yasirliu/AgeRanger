using AgeRanger.Application.Interfaces;
using AgeRanger.Command.PersonCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Application.Contracts
{
    public interface IPersonCommandServiceContract: 
        IApplicationCommandService<CreateNewPersonCommand>,
        IApplicationCommandService<ModifyExistingPersonCommand>
    {
    }
}
