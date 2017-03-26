using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event
{
    public interface IEvent
    {
        string EventId { get; }
    }
}
