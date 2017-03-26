using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Event
{
    public class ExceptionEvent : Exception, IEvent
    {
        public ExceptionEvent(string errorMessage) : base(errorMessage)
        { }

        public string EventId
        {
            get
            {
                return $"{this.GetType().Name}-{DateTime.Now.Ticks}";
            }
        }

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
