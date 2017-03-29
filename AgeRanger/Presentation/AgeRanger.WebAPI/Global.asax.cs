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
using AgeRanger.ErrorHandler.Contracts;
using AgeRanger.Domain.ServiceBus.Interfaces;
using AgeRanger.ErrorHandler.WebApiFilters;
using System.Runtime.ExceptionServices;
using AgeRanger.Event.Exceptions;

namespace AgeRanger.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            
            //Manually set the path of "SQLite.Interop.dll" which is a unmanagable dll
            int wsize = IntPtr.Size;
            string libdir = (wsize == 4) ? "x86" : "x64";
            string appPath = System.IO.Path.GetDirectoryName($@"{AppDomain.CurrentDomain.BaseDirectory}\bin\");
            SetDllDirectory(System.IO.Path.Combine(appPath, libdir));

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
                builder.RegisterType<UnKnownErrorHandler>()
                    .As<IUnKnownErrorHandler>();
                builder.RegisterType<NegativeErrorHandler>()
                    .As<INegativeErrorHandler>();

                //Register LoggerController
                builder.RegisterType<LoggerFactory>()
                    .As<ILoggerFactory>();
                builder.RegisterType<AL.LoggerController<VersionedEvent>>()
                    .As<AL.ILoggerController<VersionedEvent>>();
                builder.RegisterType<AL.LoggerController<ExceptionEvent>>()
                    .As<AL.ILoggerController<ExceptionEvent>>();
                builder.RegisterType<AL.LoggerController<UnKnownErrorEvent>>()
                    .As<AL.ILoggerController<UnKnownErrorEvent>>();
                builder.RegisterType<AL.LoggerController<FirstChanceExceptionEventArgs>>()
                    .As<AL.ILoggerController<FirstChanceExceptionEventArgs>>();

                //Register ActionFilters
                builder.RegisterType<NegativeErrorExceptionFilter>()
                    .AsWebApiExceptionFilterFor<ApiControllerExtension>()
                    .InstancePerRequest();
                builder.RegisterType<UnKnownErrorExceptionFilter>()
                    .AsWebApiExceptionFilterFor<ApiControllerExtension>()
                    .InstancePerRequest();

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
            EventBus.Instance.Subscribe(
                provider.GetContainer().Resolve<IUnKnownEventsHandler>());
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (!(e.Exception is UnKnownErrorException) && !(e.Exception is NegativeErrorException))
            {
                //Do something here to monitor all hidden exceptions which are not handled appropriately.
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);
    }
}
