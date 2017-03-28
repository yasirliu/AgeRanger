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
using AgeRanger.Domain.ServiceBus.EventBus;
using AgeRanger.Event.PersonEvent;
using AgeRanger.DIManager;
using Autofac;
using AgeRanger.Event.Contracts;

namespace AgeRanger.Command.PersonCommand
{
    /// <summary>
    /// Commands handler used to deal with person related commands
    /// The validation of properties of command is intercepted at runtime
    /// </summary>
    public class PersonCommandHandler :
        CommandHandler<Person>,
        IPersonCommandHandler
    {
        /// <summary>
        /// Inject event handlers
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="auditEventHandler"></param>
        public PersonCommandHandler(
            IPersonWriterRepositoryContract repository,
            IAuditVersionedEventHandler auditEventHandler,
            INegativeEventsHandler errorEventHandler) : base(repository)
        {
            //Subscribe event handler for audit
            EventBus.Instance.Subscribe<PersonUpdatedEvent>
                (auditEventHandler);
            //Subscribe event handler for audit
            EventBus.Instance.Subscribe<PersonCreatedEvent>
                (auditEventHandler);

            //Subscribe event handler for error - PersonNotCreatedEvent
            EventBus.Instance.Subscribe<PersonNotCreatedEvent>
                (errorEventHandler);
            //Subscribe event handler for error - PersonNotUpdatedEvent
            EventBus.Instance.Subscribe<PersonNotUpdatedEvent>
                (errorEventHandler);
        }

        public virtual void Handle(ModifyExistingPersonCommand command)
        {
            var person = this.CommandMapper<ModifyExistingPersonCommand>(command);
            _repository.Update(person, person.RowVersion);
            //Trigger event
            EventBus.Instance.Trigger(new PersonUpdatedEvent()
            {
                Id = command.Id, Age = command.Age, FirstName = command.FirstName,
                LastName = command.LastName, EventVersion = command.CommandVersion
            });
        }

        public virtual void Handle(CreateNewPersonCommand command)
        {
            var person = this.CommandMapper<CreateNewPersonCommand>(command);
            _repository.Create(person);
            //Trigger event
            EventBus.Instance.Trigger(new PersonCreatedEvent()
            {
                Age = command.Age,
                FirstName = command.FirstName,
                LastName = command.LastName,
                EventVersion = command.CommandVersion
            });

        }

        public virtual async Task HandleAsync(ModifyExistingPersonCommand command)
        {
            var person = this.CommandMapper<ModifyExistingPersonCommand>(command);
            _repository.Update(person, person.RowVersion);
            await _repository.CommitAsync().ContinueWith((task)=> {
                //Trigger event
                EventBus.Instance.Trigger<PersonUpdatedEvent>(new PersonUpdatedEvent()
                {
                    Id = command.Id,
                    Age = command.Age,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    EventVersion = command.CommandVersion
                });
            });
        }

        public virtual async Task HandleAsync(CreateNewPersonCommand command)
        {
            var person = this.CommandMapper<CreateNewPersonCommand>(command);
            _repository.Create(person);
            await _repository.CommitAsync().ContinueWith((task)=>{
                //Trigger event
                EventBus.Instance.Trigger(new PersonCreatedEvent()
                {
                    Age = command.Age,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    EventVersion = command.CommandVersion
                });
            });
        }

        protected override Person CommandMapper<TCommand>(TCommand command)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TCommand, Person>()
                .ForMember(person => person.RowVersion, opt => opt.MapFrom(src => src.CommandVersion)));

            return Mapper.Map<Person>(command);
        }
    }
}
