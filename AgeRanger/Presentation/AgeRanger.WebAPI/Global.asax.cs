using AgeRanger.Application.CommandServices;
using AgeRanger.Application.Contracts;
using AgeRanger.Application.QueryServices;
using AgeRanger.Command.Contracts;
using AgeRanger.Command.PersonCommand;
using AgeRanger.DIManager;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AgeRanger.Command.CommandValidaters;
using AgeRanger.ErrorHandler;
using AL = AgeRanger.Logger;
using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Security.WebApiFilters;
using AgeRanger.WebAPI.Base;
using Microsoft.Extensions.Logging;
using AgeRanger.Domain.ServiceBus.EventBus;
using AgeRanger.Event;
using AgeRanger.Event.Contracts;

namespace AgeRanger.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Ioc configue
            var provider = new AutofacProvider(
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\repoconfig\autofac.repo.reader.json",
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\repoconfig\autofac.repo.writer.json",
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\moduleconfig\autofac.modules.json");

            //add the configues out of configue files in PreBuild
            provider.PreBuild((builder) => {

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

                var config = GlobalConfiguration.Configuration;
                // Register your Web API controllers.
                builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

                // Register the Autofac filter provider.
                builder.RegisterWebApiFilterProvider(config);

                //Register ErrorHandler
                builder.RegisterType<NegativeErrorHandler>()
                    .As<IErrorHandler>();
                //Register LoggerController
                builder.RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>();
                builder.RegisterType<AL.LoggerController<ExceptionEvent>>()
                    .As<AL.ILoggerController<ExceptionEvent>>();
                builder.RegisterType<AL.LoggerController<VersionedEvent>>()
                    .As<AL.ILoggerController<VersionedEvent>>();
                //Register ActionFilters
                builder.RegisterType<ExceptionFilter>()
                    .AsWebApiExceptionFilterFor<ApiControllerExtension>();

            });

            provider.Build();

            provider.AfterBuild((container) =>
            {
                // Set the dependency resolver to be Autofac.
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            });
            
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //Subscribe UnKnowErrorEvent
            EventBus.Instance.Subscribe<UnKnownErrorEvent>(
                provider.GetContainer().Resolve<INegativeEventsHandler>());
        }
    }
}
