using AgeRanger.Application.CommandServices;
using AgeRanger.Application.Contracts;
using AgeRanger.Application.QueryServices;
using AgeRanger.Command.CommandValidaters;
using AgeRanger.Command.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DataContracts.Repositories;
using AgeRanger.DIManager;
using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.ErrorHandler;
using AgeRanger.ErrorHandler.Contracts;
using AgeRanger.Event;
using AgeRanger.Logger;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Application.UnitTest
{
    [TestFixture]
    public class PersonCommandTest
    {
        private IDIProvider<ContainerBuilder, IContainer> iocProvider;
        private IPersonCommandServiceContract handler;

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


                //Application services
                builder.RegisterType<PersonQueryService>()
                    .As<IPersonQueryServiceContract>()
                    .EnableInterfaceInterceptors();
                builder.RegisterType<PersonCommandService>()
                    .As<IPersonCommandServiceContract>()
                    .EnableInterfaceInterceptors();


                //Register ErrorHandler
                builder.RegisterType<UnKnownErrorHandler>()
                    .As<IUnKnownErrorHandler>();
                builder.RegisterType<NegativeErrorHandler>()
                    .As<INegativeErrorHandler>();

                //Register LoggerController
                builder.RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>();
                builder.RegisterType<LoggerController<VersionedEvent>>()
                    .As<ILoggerController<VersionedEvent>>();
                builder.RegisterType<LoggerController<ExceptionEvent>>()
                    .As<ILoggerController<ExceptionEvent>>();
                builder.RegisterType<LoggerController<UnKnownErrorEvent>>()
                    .As<ILoggerController<UnKnownErrorEvent>>();
            });

            iocProvider.Build();
            handler = iocProvider.GetContainer().Resolve<IPersonCommandServiceContract>();
        }

        [SetUp]
        public void Init()
        {
            iocProvider.GetContainer().Resolve<IPersonReaderRepositoryContract>().Query();
        }


        [Test]
        [TestCase("CreatedPerson", "CreatedPerson", 56)]
        public void Application_Person_Create_Test(string firstName, string lastName, int age)
        {
            handler.Apply(new CreateNewPersonCommand()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            });
        }


        [Test]
        [TestCase(1, "ModifiedPerson", "ModifiedPerson", 56)]
        public void Application_Person_Modify_Test(int id, string firstName, string lastName, int age)
        {
            handler.Apply(new CreateNewPersonCommand()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            });

            handler = iocProvider.GetContainer().Resolve<IPersonCommandServiceContract>();

            handler.Apply(new ModifyExistingPersonCommand()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Age = age
            });
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
