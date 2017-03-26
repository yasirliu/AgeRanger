using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Command.PersonCommand;
using Autofac.Extras.DynamicProxy;
using AgeRanger.Command.Contracts;
using AgeRanger.Command.CommandValidaters;

namespace AgeRanger.DIManager
{
    public class AutofacProvider : IDIProvider<IContainer>
    {
        private ConfigurationBuilder _config;
        private ContainerBuilder _builder;

        public AutofacProvider()
        {
            _config = new ConfigurationBuilder();
            _builder = new ContainerBuilder();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="configFiles"></param>
        /// <returns></returns>
        public virtual void ConfigureDI(params string[] configFiles)
        {
            //Autofac setup
            foreach (var file in configFiles)
            {
                _config.AddJsonFile(file);
                var module = new ConfigurationModule(_config.Build());
                _builder.RegisterModule(module);
            }
        }

        public virtual void ConfigureInterceptor()
        {
            //Interceptor only can be configured by code
            _builder.RegisterType<PersonCommandHandler>()
                .As<IPersonCommandHandler>()
                .EnableInterfaceInterceptors();
            _builder.Register(c => new CommandPropertyValidator());
        }

        public IContainer Build(params string[] configFiles)
        {
            ConfigureDI(configFiles);
            ConfigureInterceptor();
            return _builder.Build();
        }
    }
}
