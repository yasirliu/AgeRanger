using AgeRanger.ErrorHandler.Contracts;
using AgeRanger.Event.Exceptions;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net.Http;
using System.Web.Mvc;
using System.Collections;

namespace AgeRanger.ErrorHandler.WebApiFilters
{
    public class NegativeErrorExceptionFilter : IAutofacExceptionFilter
    {
        private INegativeErrorHandler _handler;
        public NegativeErrorExceptionFilter(INegativeErrorHandler handler)
        {
            _handler = handler;
        }

        public Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            if (!(actionExecutedContext.Exception is NegativeErrorException))
            {
                return Task.FromResult(0);
            }
            return Task.Run(() => {
                _handler.Handle(actionExecutedContext.Exception as NegativeErrorException);
                actionExecutedContext.Response =
                    actionExecutedContext.Request.CreateResponse<IDictionary>
                            (HttpStatusCode.BadRequest, actionExecutedContext.Exception.Data);
            });
        }
    }
}
