using AgeRanger.Application.Interfaces;
using AgeRanger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Application.Contracts
{
    public interface IPersonQueryServiceContract : IApplicationQueryService<PersonAgeGroupDto>
    {
    }
}
