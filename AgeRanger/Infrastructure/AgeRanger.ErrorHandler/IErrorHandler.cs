using AgeRanger.Domain.ServiceBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.ErrorHandler
{
    public interface IErrorHandler
    {
        /// <summary>
        /// Deal with exceptions threw by server not application
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="log"></param>
        void Handle(Exception ex);
    }
}
