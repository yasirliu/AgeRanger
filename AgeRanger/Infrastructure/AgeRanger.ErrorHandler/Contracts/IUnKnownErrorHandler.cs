using AgeRanger.Event;
using AgeRanger.Event.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.ErrorHandler.Contracts
{
    public interface IUnKnownErrorHandler : IErrorHandler<UnKnownErrorEvent, Exception>
    {
    }
}
