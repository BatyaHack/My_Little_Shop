using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Models.Entities;

namespace SportsStore.Binders
{
    //Создаем отдельный класс для хранения сесси. Так как нам не удобно польвзоваться стандартным обслуживанем сессий
    //Данный клас является процессом связывания
    public class CartModelBinder : IModelBinder
    {

        public const string SeesionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart session = (Cart)controllerContext.HttpContext.Session[SeesionKey];

            if(session == null)
            {
                session = new Cart();
                controllerContext.HttpContext.Session[SeesionKey] = session;
            }

            return session;
        }

        // Теперь нам надо указать, что мы хотим использовать данный класс для создания экземпляров Cart 
        // в Global.asax

        //Использование подобного механизма связывания дает нам несколько преимуществ.Первое
        //заключается в том, что мы отделили логику для создания объектов Cart от контроллера, что
        //позволит нам изменять способ сохранения этих объектов без необходимости изменять контроллер.
        //Вторым преимуществом является то, что любой класс контроллера, который работает с объектами
        //Cart, может просто объявить их как параметры метода действия и воспользоваться
        //пользовательским механизмом связывания.Третье и, на наш взгляд, самое главное преимущество
        //состоит в том, что теперь мы сможем тестировать контроллер Cart, не создавая имитаций
        //встроенных возможностей ASP.NET.

        //Для данного механизма связывание есть тесты (но тесты созданны для контроллера)
    }
}