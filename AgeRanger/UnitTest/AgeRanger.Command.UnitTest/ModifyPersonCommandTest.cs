using AgeRanger.Command.CommandValidaters;
using AgeRanger.Command.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DIManager;
using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.ErrorHandler;
using AgeRanger.Event.PersonEvent;
using AgeRanger.Logger;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;

namespace AgeRanger.Command.UnitTest
{
    [TestFixture]
    public class ModifyPersonCommandTest
    {
        private IDIProvider<ContainerBuilder, IContainer> iocProvider;
        private IPersonCommandHandler handler;

        [OneTimeSetUp]
        public void SetUp()
        {
            iocProvider = new AutofacProvider($@"{ AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.reader.json",
                       $@"{AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.writer.json",
                       $@"{AppDomain.CurrentDomain.BaseDirectory}moduleconfig\autofac.modules.json");

            //add the configues out of configue files in PreBuild
            iocProvider.PreBuild((builder) => {

                //Interceptor only can be configured by code
                //PersonCommandHandler dependents object that registered in config file
                builder.RegisterType<PersonCommandHandler>()
                    .As<IPersonCommandHandler>()
                    .EnableInterfaceInterceptors();
                builder.Register(c => new CommandPropertyValidator());


                //Register ErrorHandler
                builder.RegisterType<UnKnownErrorHandler>()
                    .As<IErrorHandler>();
                //Register LoggerController
                builder.RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>();
                builder.RegisterType<LoggerController<ExceptionEvent>>()
                    .As<ILoggerController<ExceptionEvent>>();
                builder.RegisterType<LoggerController<VersionedEvent>>()
                    .As<ILoggerController<VersionedEvent>>();
            });

            iocProvider.Build();
            handler = iocProvider.GetContainer().Resolve<IPersonCommandHandler>();
        }

        [Test]
        public void Update_Person_Invalid_Age()
        {
            var person = new ModifyExistingPersonCommand() { Id = 1, FirstName = "1", LastName = "2", Age = -1 };
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(person, new ValidationContext(person), result);
            IsTrue(result.First().MemberNames.First() == nameof(person.Age));
        }

        [Test]
        public void Update_Person_Invalid_FirstName()
        {
            var person = new ModifyExistingPersonCommand() { Id = 1, LastName = "2", Age = 1 };
            var result = person.Validate(new ValidationContext(person));
            IsTrue(result.First().MemberNames.First() == nameof(person.FirstName));
        }

        [Test]
        public void Update_Person_Invalid_Id()
        {
            var person = new ModifyExistingPersonCommand() { Id = 0, LastName = "2", Age = 1, FirstName="2" };
            var result = person.Validate(new ValidationContext(person));
            IsTrue(result.First().MemberNames.First() == nameof(person.Id));
        }

        [Test]
        public void Update_Person_Valid_Entity()
        {
            ThrowsAsync<DbUpdateConcurrencyException>(async delegate
            {
                var person = new ModifyExistingPersonCommand() { Id = 1, FirstName = "Adam111111", LastName = "Liu11111", Age = 1000 };
                await handler.HandleAsync(person);
            });
            
        }

        [Test]
        public void Update_Person_InValid_Entity()
        {
            //ThrowsAsync<PersonNotCreatedEvent>(async delegate
            //{
            //    var person = new CreateNewPersonCommand() { Age = 100 };
            //    await handler.HandleAsync(person);
            //});
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            iocProvider.Dispose();
            iocProvider = null;
        }
    }
}
