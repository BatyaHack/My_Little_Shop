using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Models.Abstract;

namespace SportsStore.Controllers
{
    public class NavController : Controller
    {
        //Данный котролер является дочерним. То есть он используеся в других представления, что бы вывести
        // какуюто часть логики и представления.

        /* Дочернее действие полагается на вспомогательный метод HTML под названием RenderAction,
            который позволяет включить вывод из произвольного метода действия в текущее представление. В
            этом случае мы можем создать новый контроллер (назовем его NavController) с методом действия
            (в данном случае Menu), который визуализирует меню навигации и внедряет вывод из данного метода
            в макет.
            Такой подход дает нам реальный контроллер, который может содержать любую необходимую нам
            логику приложения, и который может быть протестирован, как и любой другой контроллер. Это
            действительно хороший способ создания небольших сегментов приложения, при котором
            сохраняется общий подход MVC Framework.
         */
        // GET: Nav

        private IProductsRepository repository;

        public NavController(IProductsRepository repo)
        {
            this.repository = repo;
        } 


        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);

            return PartialView(categories);
        }
            //ЕСТЬ ДВА МОДУЛЬНЫХ ТЕСТА    


            //Мы хотим, чтобы список категории появлялся на всех
            //страницах, так что мы собирается визуализировать дочернее действие в макете, а не в определенном
            //представлении
    }
}