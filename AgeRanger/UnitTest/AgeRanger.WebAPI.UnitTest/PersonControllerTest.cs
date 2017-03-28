using AgeRanger.Application.CommandServices;
using AgeRanger.Application.Contracts;
using AgeRanger.Application.QueryServices;
using AgeRanger.Command.CommandValidaters;
using AgeRanger.Command.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DIManager;
using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.ErrorHandler;
using AgeRanger.Logger;
using AgeRanger.Security.WebApiFilters;
using AgeRanger.WebAPI.Base;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.WebApi;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.WebAPI.UnitTest
{
    [TestFixture]
    public class PersonControllerTest
    {
        private IDIProvider<ContainerBuilder, IContainer> iocProvider;

        [OneTimeSetUp]
        public void SetUp()
        {
            iocProvider = new AutofacProvider(
                $@"{ AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.reader.json",
                $@"{AppDomain.CurrentDomain.BaseDirectory}repoconfig\autofac.repo.writer.json");

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
                builder.RegisterType<NegativeErrorHandler>()
                    .As<IErrorHandler>();
                //Register LoggerController
                builder.RegisterType<LoggerController<ExceptionEvent>>()
                    .As<ILoggerController<ExceptionEvent>>();
                //Register ActionFilters
                builder.RegisterType<ExceptionFilter>()
                    .AsWebApiExceptionFilterFor<ApiControllerExtension>();
            });

            iocProvider.Build();
        }
    }
}
