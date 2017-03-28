using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Event.Contracts;
using AgeRanger.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
            if (!(@event is UnKnownErrorEvent))
            {
                //log
                _controller.Logger.LogError(@event.ToString());
            }
            else
            {
                //log
                _controller.Logger.LogCritical(@event.ToString());
            }
            throw new Exception(@event.ErrorMessage) { Source = @event.GetType().Name };
        }

        public override async Task HandleAsync(ExceptionEvent @event)
        {
            //log
            await Task.Factory.StartNew(() => {
                if (!(@event is UnKnownErrorEvent))
                {
                    _controller.Logger.LogError(@event.ToString());
                }
                else
                {
                    //log
                    _controller.Logger.LogCritical(@event.ToString());
                }
                throw new Exception(@event.ErrorMessage) { Source = @event.GetType().Name };
            });
        }
    }
}
