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
        private IOrderProcessor orderProcessor;

        public CartController(IProductsRepository repo, IOrderProcessor pocess)
        {
            repository = repo;
            orderProcessor = pocess;
        }

        public ViewResult Index(Cart cart ,string returnUrl)
        {
            return View(new CartIndexViewModel {
                //Cart = GetCart(), ***
                Cart = cart,
                ReturnUrl = returnUrl
            } );
        }

        //Метод для представления корзины
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails) //для этого метода есть тект
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("","Ваша карзина пуста");
            }

            if (ModelState.IsValid) //если нет ошибок
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Complere");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public ViewResult Checkout() //Вызываем форму для заполнения покупок
        {
            return View(new ShippingDetails());
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                //GetCart().AddItem(product, 1); *** 
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                //GetCart().RemoveLine(product); ***
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }


        #region До добавление свясвязи между классом Cart и CartModleBinder ***
        //private Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

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
        #endregion


    }
}