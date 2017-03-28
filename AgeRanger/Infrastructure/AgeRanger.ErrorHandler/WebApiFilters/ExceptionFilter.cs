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

namespace AgeRanger.Security.WebApiFilters
{
    public class ExceptionFilter : IAutofacExceptionFilter
    {
        private IErrorHandler _handler;
        public ExceptionFilter(IErrorHandler handler)
        {
            _handler = handler;
        }

        public Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, 
            CancellationToken cancellationToken)
        {
            return Task.Run(() => _handler.Handle(actionExecutedContext.Exception)).ContinueWith((task)=> {
                if (task.Exception.InnerException.Source == nameof(UnKnownErrorEvent))
                {
                    actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                }
                else
                {
                    actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            });
        }
    }
}
