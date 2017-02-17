using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        //Данный контроллер используется бля администрирования
        //То есть он предоставляет нам методу CRUD - создавать читать обновлять удалять
        // То есть работаем уже непосредственно с БД
        // GET: Admin

        public IProductsRepository repository;

        public AdminController(IProductsRepository rep)
        {
            repository = rep;
        }

        public ViewResult Index() //мы должны протестировать Index на до как кон передает данные AdminTests
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int ProductID) //для этого метода есть тест
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product prod) //для этого метода есть тест
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(prod);
                TempData["message"] = string.Format("В {0} сохранение изменены", prod.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(prod);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
            // Так как после отправки данных в предаставление Edit и заново отпарвки формы через кнопку
            // будет вызывается метод Create, а для обработки и сохранения должен использоваться
            // HttpPost Edit, так что нужно подправить предаствление Edit
        }

        [HttpPost] //ддя данного метода есть тест
        public ActionResult Delete(int productID)
        {
            Product delereProduct = repository.DeleteProduct(productID);
            if (delereProduct != null)
            {
                TempData["message"] = string.Format("{0} был удален", delereProduct.Name);
            }

            return RedirectToAction("Index");
        }

        #region что такое TempData
        /*
                Мы сохраняем сообщение с помощью объекта TempData. Этот набор пар ключ/значение похож на данные сессии и ViewBag, которые мы использовали ранее.
        Его основное отличие от данных сессии заключается в том, что TempData удаляется в конце запроса HTTP.

                В этой ситуации мы не можем использовать ViewBag, так как нам нужно перенаправить
        пользователя. ViewBag передает данные между контроллером и представлением, и он не может
        хранить данные дольше, чем длится текущий запрос HTTP. Мы могли бы использовать данные
        сессии, но тогда сообщение будет храниться, пока мы явно его не удалим, а нам не хочется этого
        делать. Таким образом, TempData нам идеально подходит. Данные ограничиваются сессией одного
        пользователя (так что пользователи не видят другие TempData) и будут сохранены до тех пор, пока
        мы их не прочитаем. Они понадобятся нам в представлении, визуализированном тем методом
        действия, к которому мы перенаправили пользователя.
         */
        #endregion

    }
}