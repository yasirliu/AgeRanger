using AgeRanger.DIManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AgeRanger.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //Ioc configue
            new AutofacProvider(
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\repoconfig\autofac.repo.reader.json",
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\repoconfig\autofac.repo.writer.json")
                .Build();
        }
    }
}
