using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event
{
    public class VersionedEvent : IEvent
    {
        public string EventId
        {
            get {
                return $"{this.GetType().Name}-{DateTime.Now.Ticks}";
            }
        }

        public byte[] EventVersion { get; set; }
    }
}
