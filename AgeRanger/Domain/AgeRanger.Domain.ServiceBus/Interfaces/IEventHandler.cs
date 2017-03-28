﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Domain.ServiceBus.Interfaces
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
        Task HandleAsync(TEvent @event);
        int OrderId { get; set; }
    }
}
