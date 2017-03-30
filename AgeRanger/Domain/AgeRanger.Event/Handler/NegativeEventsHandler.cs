using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Event.Contracts;
using AgeRanger.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AgeRanger.Event.Exceptions;

namespace AgeRanger.Event.Handler
{
    public class NegativeEventsHandler : GenericEventHandler<ExceptionEvent>, INegativeEventsHandler
    {
        private ILoggerController<ExceptionEvent> _controller;
        public NegativeEventsHandler(ILoggerController<ExceptionEvent> controller)
        {
            _controller = controller;
        }

        public override void Handle(ExceptionEvent @event)
        {
            //log
            _controller.Logger.LogError(@event.ToString());

            var ex = new NegativeErrorException(@event.EventId.ToString(), @event.ErrCode);
            throw ex;
        }

        public override async Task HandleAsync(ExceptionEvent @event)
        {
            //log
            await Task.Factory.StartNew(() => {
                _controller.Logger.LogError(@event.ToString());
                var ex = new NegativeErrorException(@event.EventId.ToString(), @event.ErrCode);
                throw ex;
            });
        }
    }
}
