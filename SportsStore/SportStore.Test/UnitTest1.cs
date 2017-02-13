using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc; //Нужно ставить через nuget Asp.Net MVC
using SportsStore.HtmlHelpers;

namespace SportStore.Test
{
    [TestClass]
    public class UnitTest1
    {
        ////Тест который проверяет с помошью заглушки метод, который распалагает товар по страницам
        //[TestMethod]
        //public void Can_Paginate() //Данный тет проверят регион с 2 в контролере
        //{
        //    // Arrange
        //    Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

        //    mock.Setup(m => m.Products).Returns(new Product[] {
        //             new Product {ProductID = 1, Name = "P1"},
        //             new Product {ProductID = 2, Name = "P2"},
        //             new Product {ProductID = 3, Name = "P3"},
        //             new Product {ProductID = 4, Name = "P4"},
        //             new Product {ProductID = 5, Name = "P5"}
        //    }.AsQueryable());

        //    ProductController controller = new ProductController(mock.Object);

        //    controller.PageSize = 3;

        //    // Act
        //    //Теперь получим модель, которую передает View в представление
        //    IEnumerable<Product> result = (IEnumerable<Product>)controller.List(2).Model;
        //    /*
        //     Обратите внимание, как легко можно получить данные, возвращаемые из метода контроллера. Мы
        //     вызываем свойство Model в результате, чтобы получить последовательность IEnumerable<Product>,
        //     которую мы генерировали в методе List. 
        //     */

        //    // Assert
        //    Product[] proadArray = result.ToArray();
        //    Assert.IsTrue(proadArray.Length == 2);
        //    Assert.AreEqual(proadArray[0].Name, "P4");
        //    Assert.AreEqual(proadArray[1].Name, "P5");
        //}

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
                 + @"<a class=""selected"" href=""Page2"">2</a>"
                 + @"<a href=""Page3"">3</a>");

            #region Совет
            //Этот тест проверяет вывод вспомогательного метода, используя значение литеральной строки,
            //которая содержит двойные кавычки. С# прекрасно работает с такими строками до тех пор, пока мы
            //ставим перед строкой @ и используем два набора двойных кавычек ("") вместо одного набора.Мы
            //должны также помнить, что нельзя разбивать литеральную строку на отдельные строки, если только
            //строка, с которой мы сравниваем, не разбита аналогично.Так, например, литерал, который мы
            //используем в тестовом методе, занял две строки из - за небольшой ширины страницы.Мы не
            //добавили символ новой строки; если бы мы это сделали, тест завершился бы неудачей.
            #endregion
        }

        //Проверяем правильную ли информацию передает View  в модель
        // В данном случае проверям PagingInfo из ProductsListViewModel
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                 new Product {ProductID = 1, Name = "P1"},
                 new Product {ProductID = 2, Name = "P2"},
                 new Product {ProductID = 3, Name = "P3"},
                 new Product {ProductID = 4, Name = "P4"},
                 new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List().Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 1);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
        }


        // Проверяем правильную ли информацию передает View  в модель
        // В данном случае проверям Prodcut из ProductsListViewModel (правильно ли он раскидывает Product по страницам)
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                 new Product {ProductID = 1, Name = "P1"},
                 new Product {ProductID = 2, Name = "P2"},
                 new Product {ProductID = 3, Name = "P3"},
                 new Product {ProductID = 4, Name = "P4"},
                 new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List().Model;

            Product[] arry = result.Products.ToArray();
            Assert.IsTrue(arry.Length == 3);
            Assert.AreEqual(arry[0].Name, "P1");
            Assert.AreEqual(arry[1].Name, "P2");
            Assert.AreEqual(arry[2].Name, "P3");
        }
    }
}
