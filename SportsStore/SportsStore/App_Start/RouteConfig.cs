using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}"); //Ингнорируем данные URLы

            #region v 1.0.
            ////Настройка новый маршрутизации
            // В первой версии
            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new { Controller = "Product", action = "List" }
            //    );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Product", action = "List", id = UrlParameter.Optional }
            //);
            #endregion


            //Роут по умолчанию. (Как только мы сразу саходим на сайт)
            routes.MapRoute(null, "", new
            {
                Controller = "Product",
                action = "List",
                category = (string)null,
                page = 1
            }
            );

            //Роут когда мы передаем в него именовыный параметр Page
            routes.MapRoute(null, "Page{page}",
                new { controller = "Product", action = "List", category = (string)null },
                new { page = @"\d+" }
                );

            //Роует когда мы передаем в него Category
            routes.MapRoute(null,
                "{category}",
                new { controller = "Product", action = "List", page = 1 }
            );

            //Роутер когда мы передаем в него и Page и Category

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Product", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");

            /*
             Вот так будет выглядить система URL в браузере
                /  Выводит список товаров из всех категорий для первой страницы.
                /Page2 Выводит список товаров из всех категорий для указанной страницы 
                /Soccer Показывает первую страницу товаров из определенной категории 
                /Soccer/Page2 Показывает указанную страницу (в данном случае 2) товаров из указанной категории (в данном случае Soccer).
                /Anything/Else Вызывает метод действия Else контроллера Anything.
           */

        }
    }
}
