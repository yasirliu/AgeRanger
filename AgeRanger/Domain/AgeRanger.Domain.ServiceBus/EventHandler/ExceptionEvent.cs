using AgeRanger.Domain.ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Domain.ServiceBus.EventHandler
{
    public abstract class ExceptionEvent : Exception, IEvent
    {
        public ExceptionEvent(string errorMessage) : base(errorMessage)
        { }

        public Guid EventId
        {
            get;
        } = Guid.NewGuid();

        /// <summary>
        /// Error code would be used in web app
        /// </summary>
        public string ErrCode
        {
            get
            {
                return $"{this.GetType().Name}";
            }
        }
    }
}
