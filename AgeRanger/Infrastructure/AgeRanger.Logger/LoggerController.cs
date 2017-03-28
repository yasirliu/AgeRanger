using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace AgeRanger.Logger
{
    /// <summary>
    /// Control logger configuration and logger provider under .NET core logging extension
    /// </summary>
    /// <typeparam name="TCategory"></typeparam>
    public class LoggerController<TCategory> : ILoggerController<TCategory>
    {
        private readonly ILoggerFactory _factory;

        public LoggerController(ILoggerFactory factory)
        {
            _factory = factory;
            factory.AddNLog();
            //add more providers... 
            Logger = factory.CreateLogger<TCategory>();
        }

        public ILogger<TCategory> Logger
        {
            get;
        }
    }
}
