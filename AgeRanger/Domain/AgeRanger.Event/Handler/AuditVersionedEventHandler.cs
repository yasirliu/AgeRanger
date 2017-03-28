using AgeRanger.Domain.ServiceBus.EventHandler;
using AgeRanger.Event.Contracts;
using AgeRanger.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event.Handler
{
    public sealed class AuditVersionedEventHandler : GenericEventHandler<VersionedEvent>, IAuditVersionedEventHandler
    {
        private ILoggerController<VersionedEvent> _controller;
        public AuditVersionedEventHandler(ILoggerController<VersionedEvent> controller)
        {
            _controller = controller;
        }

        public override void Handle(VersionedEvent @event)
        {
            //log audit
        }

        public override Task HandleAsync(VersionedEvent @event)
        {
            //log audit
            return Task.FromResult(0);
        }
    }
}
