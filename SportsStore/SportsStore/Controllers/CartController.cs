using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductsRepository repository;

        public CartController(IProductsRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            } );
        }

        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                GetCart().RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        /*По поводу этого контроллера есть несколько замечаний. Первое касается того, что мы используем
состояние сессии ASP.NET для сохранения и извлечения объектов Cart. Это задача метода GetCart.
В ASP.NET есть объект Session, который использует cookie или перезапись URL для группировки
запросов от пользователя, чтобы сформировать одну сессию просмотра. Состояние сессии (session
state) позволяет связывать данные с сессией. Оно идеально подходит для нашего класса Cart. Мы
хотим, чтобы у каждого пользователя была своя корзина, и чтобы она сохранялась в промежутках
времени между запросами. Данные, которые связываются с сессией, удаляются, когда сессия
истекает (обычно потому, что пользователь не отправлял запросы некоторое время). Это означает,
что мы не должны управлять хранением или жизненным циклом объектов Cart.

    Последнее замечание по поводу контроллера Cart состоит в том, что и метод AddToCart, и
RemoveFromCart вызывают метод RedirectToAction. В результате этого браузеру клиента
отправляется HTTP-инструкция перенаправления, которая сообщает браузеру запросить новый URL.
В этом случае мы сообщаем браузеру запросить URL, который будет вызывать метод действия Index
контроллера Cart.*/
    }
}