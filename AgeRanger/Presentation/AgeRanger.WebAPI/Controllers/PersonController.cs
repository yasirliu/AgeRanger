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
        private IPersonQueryServiceContract _queryService;
        private IPersonCommandServiceContract _commandService;

        public PersonController(IPersonQueryServiceContract queryService,
            IPersonCommandServiceContract commandService)
        {
            _queryService = queryService;
            _commandService = commandService;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<PersonAgeGroupDto>> GetPersons(
            string filter = null,
            string orderBy = null,
            int? pageIndex = null,
            int? pageCount = null)
        {
            var result = await _queryService.Query(filter, orderBy, pageIndex, pageCount);
            return result;
        }

        // GET api/<controller>/5
        [HttpGet]
        public async Task<PersonAgeGroupDto> GetPerson(int Id)
        {
            var result = await _queryService.GetById(Id);
            return result;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task CreatePerson(CreateNewPersonCommand command)
        {
            await _commandService.ApplyAsync(command);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task EditPerson(ModifyExistingPersonCommand command)
        {
            await _commandService.ApplyAsync(command);
        }

    }
}