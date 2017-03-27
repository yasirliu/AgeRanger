﻿using Autofac;
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
using AgeRanger.Application.QueryServices;
using AgeRanger.Application.Contracts;
using AgeRanger.Application.CommandServices;

namespace AgeRanger.DIManager
{
    public class AutofacProvider : IDIProvider<IContainer>
    {
        private ConfigurationBuilder _config;
        private ContainerBuilder _builder;
        private string[] _configFiles;

        private IContainer _container;

        public AutofacProvider(params string[] configFiles)
        {
            _config = new ConfigurationBuilder();
            _builder = new ContainerBuilder();
            _configFiles = configFiles;
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

            //Interceptor only can be configured by code
            //PersonCommandHandler dependents object that registered in config file
            _builder.RegisterType<PersonCommandHandler>()
                .As<IPersonCommandHandler>()
                .EnableInterfaceInterceptors();

            //Application services
            _builder.RegisterType<PersonQueryService>()
                .As<IPersonQueryServiceContract>()
                .EnableInterfaceInterceptors();
            _builder.RegisterType<PersonCommandService>()
                .As<IPersonCommandServiceContract>()
                .EnableInterfaceInterceptors();
        }

        public virtual void ConfigureInterceptor()
        {
            _builder.Register(c => new CommandPropertyValidator());
        }

        public void Build()
        {
            if (this._container == null)
            {
                _build(_configFiles);
                _container = _builder.Build();
            }
        }

        private void _build(params string[] configFiles)
        {
            ConfigureDI(configFiles);
            ConfigureInterceptor();
        }

        public IContainer GetContainer()
        {
            if (_container == null)
            {
                throw new NullReferenceException("IoC container doesn't exit in context, execute Build() before GetContainer()");
            }
            return _container;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._container.Dispose();
                }
                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}