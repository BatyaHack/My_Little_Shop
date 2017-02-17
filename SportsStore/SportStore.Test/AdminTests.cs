using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;
using SportsStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.Test
{
    [TestClass]
    public class AdminTests
    {
        //Проверям то какие данные передает Index()
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { Name="P1", ProductID=1},
                new Product { Name="P2", ProductID=2},
                new Product { Name="P3", ProductID=3},
                new Product { Name="P4", ProductID=4}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result[0].Name, "P1");
            Assert.AreEqual(result[1].Name, "P2");
            Assert.AreEqual(result[2].Name, "P3");
            Assert.AreEqual(result[3].Name, "P4");
        }

        [TestMethod]
        public void Can_Edit_Product() //Когда мы передаем ID котоый есть в бд
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product() { Name="P1", ProductID=1},
                new Product() { Name="P2", ProductID=2},
                new Product() { Name="P3", ProductID=3},
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product p1 = (Product)target.Edit(1).ViewData.Model;
            Product p2 = (Product)target.Edit(2).ViewData.Model;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { Name ="P1", ProductID=1},
                new Product { Name ="P2", ProductID=2},
                new Product { Name ="P3", ProductID=3},
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product p1 = target.Edit(4).ViewData.Model as Product;

            Assert.IsNull(p1);
        }

        //Проверям Edit POST
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //Когда у нас все хорошо мы взываем ActionResult

            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            AdminController target = new AdminController(mock.Object);

            Product product = new Product() { Name = "Test" };

            ActionResult result = target.Edit(product);

            Assert.IsInstanceOfType(result, typeof(ActionResult)); //проверям дйствительно ли result является ActionResult
            //Assert.IsNotInstanceOfType(result, typeof(ViewResult)); то же самое что и выше, только через отрицание
        }

        //Проверям Edit POST
        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //Когда у нас все плохо мы вызываем ViewResult

            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            AdminController target = new AdminController(mock.Object);

            Product prod = new Product() { Name = "P1" };

            target.ModelState.AddModelError("error", "error");

            ActionResult result = target.Edit(prod);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult)); //проверям дйствительно ли result является ViewResult
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            Product prod = new Product() { ProductID = 2, Name = "Test" };

            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { Name= "P1", ProductID=1},
                prod,
                new Product { Name="P2", ProductID = 3}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            target.Delete(prod.ProductID);

            mock.Verify(m => m.DeleteProduct(prod.ProductID)); //удоставериваемя в том что DeleteProduct вызывается с правильным ID
        }
    }
}

