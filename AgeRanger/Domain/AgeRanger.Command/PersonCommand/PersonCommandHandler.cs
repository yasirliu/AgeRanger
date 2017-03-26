using AgeRanger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.DataContracts;
using AgeRanger.DataContracts.Repositories;
using AutoMapper;
using AgeRanger.Command.Contracts;
using Autofac.Extras.DynamicProxy;
using AgeRanger.Command.CommandValidaters;
using AgeRanger.Domain.ServiceBus.CommandHandler;

namespace AgeRanger.Command.PersonCommand
{
    /// <summary>
    /// Commands handler used to deal with person related commands
    /// The validation of properties of command is intercepted at runtime
    /// </summary>
    public class PersonCommandHandler :
        CommandHandlerBase<Person>,
        IPersonCommandHandler
    {
        public PersonCommandHandler(IPersonWriterRepositoryContract repository) : base(repository)
        {
        }

        public virtual void Handle(ModifyExistingPersonCommand command)
        {
            var person = this.CommandMapper<ModifyExistingPersonCommand>(command);
            _repository.Update(person, person.RowVersion);
            _repository.Commit();
        }

        public virtual void Handle(CreateNewPersonCommand command)
        {
            var person = this.CommandMapper<CreateNewPersonCommand>(command);
            _repository.Create(person);
            _repository.Commit();
        }

        public virtual async Task HandleAsync(ModifyExistingPersonCommand command)
        {
            var person = this.CommandMapper<ModifyExistingPersonCommand>(command);
            _repository.Update(person, person.RowVersion);
            await _repository.CommitAsync();
        }

        public virtual async Task HandleAsync(CreateNewPersonCommand command)
        {
            var person = this.CommandMapper<CreateNewPersonCommand>(command);
            _repository.Create(person);
            await _repository.CommitAsync();
        }

        protected override Person CommandMapper<TCommand>(TCommand command)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TCommand, Person>()
                .ForMember(person => person.RowVersion, opt => opt.MapFrom(src => src.CommandVersion)));

            return Mapper.Map<Person>(command);
        }
    }
}
