using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using SportsStore.Models.Entities;
using SportsStore.Binders;

using Ninject;
using SportsStore.Infrastructure;

namespace SportsStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

             
            // Говорим какую конфигурацию для созданиия классов в контролерах мы хотим использовать
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            // Указываем какой класс использовать когда надо будет создавать экземпляр Cart
            // То есть когда нужен будет экземпляр класса Cart нам нужно будет его создать по правилам класса CartModelBinder
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());

            
        }
    }
}
