using AgeRanger.DIManager;
using AgeRanger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using AgeRanger.Application.Contracts;
using System.Threading.Tasks;
using AgeRanger.Domain.ServiceBus.Interfaces;
using AgeRanger.Command.PersonCommand;

namespace AgeRanger.WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PersonController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<PersonAgeGroupDto>> GetPersons(
            string filter = null,
            string orderBy = null,
            int? pageIndex = null,
            int? pageCount = null)
        {
            using (var service = AutofacProvider.Container.Resolve<IPersonQueryServiceContract>())
            {
                var result = await service.Query(filter, orderBy, pageIndex, pageCount);
                return result;
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        public async Task<PersonAgeGroupDto> GetPerson(int Id)
        {
            using (var service = AutofacProvider.Container.Resolve<IPersonQueryServiceContract>())
            {
                var result = await service.GetById(Id);
                return result;
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task CreatePerson(CreateNewPersonCommand command)
        {
            using (var service = AutofacProvider.Container.Resolve<IPersonCommandServiceContract>())
            {
                await service.ApplyAsync(command);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task EditPerson(ModifyExistingPersonCommand command)
        {
            using (var service = AutofacProvider.Container.Resolve<IPersonCommandServiceContract>())
            {
                await service.ApplyAsync(command);
            }
        }

    }
}