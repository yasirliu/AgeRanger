﻿using AgeRanger.Command.CommandValidaters;
using AgeRanger.Command.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DataContracts.Repositories;
using AgeRanger.DIManager;
using AgeRanger.Domain.ServiceBus.EventBus;
using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.ErrorHandler;
using AgeRanger.Event.PersonEvent;
using AgeRanger.Logger;
using AgeRanger.Security.WebApiFilters;
using AgeRanger.WebAPI.Base;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;

namespace AgeRanger.Command.UnitTest
{
    [TestFixture]
    public class CreatePersonCommandTest
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
                builder.RegisterType<NegativeErrorHandler>()
                    .As<IErrorHandler>();
                //Register LoggerController
                builder.RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>();
                builder.RegisterType<LoggerController<ExceptionEvent>>()
                    .As<ILoggerController<ExceptionEvent>>();
                builder.RegisterType<LoggerController<VersionedEvent>>()
                    .As<ILoggerController<VersionedEvent>>();
            });


            iocProvider
                .Build();
            handler = iocProvider.GetContainer().Resolve<IPersonCommandHandler>();
        }

        [Test]
        public void Create_Person_Invalid_Age()
        {
            var person = new CreateNewPersonCommand() { FirstName = "1", LastName = "2", Age = -1 };
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(person, new ValidationContext(person), result);
            IsTrue(result.First().MemberNames.First() == nameof(person.Age));

        }

        [Test]
        public void Create_Person_Invalid_FirstName()
        {
            var person = new CreateNewPersonCommand() { LastName = "2", Age = 1 };
            var result = person.Validate(new ValidationContext(person));
            IsTrue(result.First().MemberNames.First() == nameof(person.FirstName));
        }

        [Test]
        public void Create_Person_Valid_Entity()
        {
            var person = new CreateNewPersonCommand() { FirstName = "Adam", LastName = "Liu", Age = 1 };
            handler.HandleAsync(person).Wait();
        }

        [Test]
        public void Create_Person_InValid_Entity()
        {
            //ThrowsAsync<PersonNotCreatedEvent>(async delegate
            //{
            //    var person = new CreateNewPersonCommand() { Age = 100 };
            //    await handler.HandleAsync(person);
            //});
        }

        [TearDown]
        public void Dispose()
        {
            iocProvider.GetContainer().Resolve<IPersonWriterRepositoryContract>().Delete(null);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            iocProvider.Dispose();
            iocProvider = null;
        }
    }
}
