using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Logger;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Domain.ServiceBus.Interfaces;
using AgeRanger.Event;
using AgeRanger.Domain.ServiceBus.EventBus;

namespace AgeRanger.ErrorHandler
{
    public class NegativeErrorHandler : IErrorHandler
    {
        private ILoggerController<ExceptionEvent> _loggerController;
        public NegativeErrorHandler(ILoggerController<ExceptionEvent> loggerController)
        {
            _loggerController = loggerController;
        }

        public void Handle(Exception ex)
        {
            //Trigger UnKnowErrorException event
            EventBus.Instance.Trigger(new UnKnownErrorEvent(ex.Message)
            {
                ErrorStack = ex.ToString()
            });
        }
    }
}
