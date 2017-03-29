using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Event.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Logger;
using AgeRanger.ErrorHandler.Contracts;

namespace AgeRanger.ErrorHandler
{
    public class NegativeErrorHandler : ErrorHandler<ExceptionEvent, NegativeErrorException>, INegativeErrorHandler
    {
        public NegativeErrorHandler(ILoggerController<ExceptionEvent> loggerController) : base(loggerController)
        {
        }

        public override void Handle(NegativeErrorException ex)
        {
            //Do something here if the error is a business rule error
        }
    }
}
