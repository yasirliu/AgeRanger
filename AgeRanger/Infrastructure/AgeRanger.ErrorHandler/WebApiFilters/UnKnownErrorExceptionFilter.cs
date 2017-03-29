using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Threading;
using System.Web.Http.Filters;
using AgeRanger.ErrorHandler;
using AgeRanger.Domain.ServiceBus.Interfaces;
using System.Net.Http;
using System.Web.Http;
using AgeRanger.Event;
using System.Net;
using System.Collections;
using AgeRanger.ErrorHandler.Contracts;
using AgeRanger.Event.Exceptions;

namespace AgeRanger.Security.WebApiFilters
{
    public class UnKnownErrorExceptionFilter : IAutofacExceptionFilter
    {
        private IUnKnownErrorHandler _handler;
        public UnKnownErrorExceptionFilter(IUnKnownErrorHandler handler)
        {
            _handler = handler;
        }

        public Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, 
            CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception is NegativeErrorException)
            {
                return Task.FromResult(0);
            }
            return Task.Run(() => _handler.Handle(actionExecutedContext.Exception)).ContinueWith((task)=> {
                //error messages are stored in Exception.Data not Exception.Message
                actionExecutedContext.Response =
                    actionExecutedContext.Request.CreateResponse<IDictionary>
                            (HttpStatusCode.InternalServerError, task.Exception.InnerException.Data);
            });
        }
    }
}
