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
    public class ProductController : Controller
    {
        private IProductsRepository repository;
        public int PageSize = 4; //Добовляем нумерацию страниц. Указываем, что у нас будет 4 ре продукта на страницу
        public ProductController(IProductsRepository productRepository)
        {
            this.repository = productRepository;
        }

        public ViewResult List(int page = 1) // ИМЕНОВАНЫЙ ПАРАМЕТР
        {
            #region 1 
            //Передаем модель
            //return View(repository.Products);
            #endregion

            #region 2

            //ЧТО БЫ ЗДЕСТЬ ПЕРЕЙТИ НА ВТОРУЮ СТРАНИЦУ В URL нужно дописать ?page=2. И мы поять вызовем List только с другим параметором

            //Реализуем нумерацию страниц
            //return View(repository.Products.OrderBy(p => p.ProductID).Skip((page-1)*PageSize).Take(PageSize));
            /*
            В методе List мы получаем
            объекты Product из хранилища, упорядочиваем их по первичному ключу, пропускаем товары,
            которые идут до начала нашей страницы, и берем количество товаров, указанное в поле PageSize

            ТАКЖЕ ДЛЯ ЭТОЙ ФУНКЦИИ ЕСТЬ НАПИСАННЫЙ ТЕСТ
             */
            #endregion

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products.OrderBy(p => p.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                }
            };

            return View(model);
        
        }

        // Также для этого кантролера настроим роут по умолчанию
        // то есть раньше мы шли на HomeController и метод  Index()
        // А после настройки роута будем идти на данный контролер и метод List()
        // делаем это в App_Start/RouteConfig.cs
    }
}