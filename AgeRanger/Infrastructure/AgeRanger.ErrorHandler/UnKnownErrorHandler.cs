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
using AgeRanger.ErrorHandler.Contracts;
using AgeRanger.Event.Exceptions;

namespace AgeRanger.ErrorHandler
{
    public class UnKnownErrorHandler : ErrorHandler<UnKnownErrorEvent, Exception>, IUnKnownErrorHandler
    {
        public UnKnownErrorHandler(ILoggerController<UnKnownErrorEvent> loggerController) : base(loggerController)
        {
        }

        public override void Handle(Exception ex)
        {
            if (ex != null)
            {
                //Trigger UnKnowErrorException event
                EventBus.Instance.Trigger(new UnKnownErrorEvent(ex.Message)
                {
                    ErrorStack = ex.ToString()
                });
            }
        }
    }
}
