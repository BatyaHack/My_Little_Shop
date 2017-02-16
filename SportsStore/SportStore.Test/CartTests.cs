using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Models.Entities;
using System.Linq;
using SportsStore.Models.Abstract;
using SportsStore.Controllers;
using System.Web.Mvc;
using SportsStore.Models;

namespace SportStore.Test
{
    [TestClass]
    public class CartTests
    {
        //Если в козине нет нужно предмета добавляем новый(проверям как работает добавление)
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            Cart.CartLine[] result = target.Lines.ToArray(); //Длжно быть подключено LINQ

            Assert.IsTrue(result.Length == 2);
            Assert.AreEqual(result[0].Product, p1);
            Assert.AreEqual(result[1].Product, p2);

        }

        /*Однако, если Product уже есть в корзине, мы хотим увеличить количество в соответствующем
объекте CartLine и не создавать новый*/
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 2);
            target.AddItem(p2, 1);

            Cart.CartLine[] result = target.Lines.ToArray();

            Assert.AreEqual(result[0].Quantity, 2);
            Assert.AreEqual(result[1].Quantity, 1);

            target.AddItem(p1, 2);
            target.AddItem(p2, 2);

            Cart.CartLine[] result2 = target.Lines.ToArray();

            Assert.AreEqual(result2[0].Quantity, 4);
            Assert.AreEqual(result2[1].Quantity, 3);
        }

        //Проверям удаление из корзины
        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            Cart target = new Cart();

            target.AddItem(p1, 2);
            target.AddItem(p2, 5);
            target.AddItem(p3, 1);

            target.RemoveLine(p2);

            Assert.AreEqual(target.Lines.Where(l => l.Product == p2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        //Проверяем как считается самму
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 2);//200
            target.AddItem(p2, 5);//250

            decimal result = target.ComputeTotalValue();

            Assert.AreEqual(result, 450M);
        }

        //Проверям полную очистку корзины
        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 1);

            target.Clear();

            Assert.AreEqual(target.Lines.Count(), 0);
        }

        //Проверяем контроллер CartController

        //Проверям провильно лии контроллер добавляем эллементы в корзину
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { Name ="Apple", ProductID = 1}
            }.AsQueryable());

            CartController target = new CartController(mock.Object, null);
            Cart cart = new Cart();
            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        //Проверяем вызывает ли контроллер метод действия Index
        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { Name="P1", ProductID=1 }
            }.AsQueryable());

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);
            RedirectToRouteResult result = target.AddToCart(cart, 1, "MyUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "MyUrl");
        }

        //Проверям URL по которому пользователь может коректно вернуться оттуда откуда пришел
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();

            CartController target = new CartController(null, null);

            CartIndexViewModel result = ((CartIndexViewModel)target.Index(cart, "MyUrl").Model);

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "MyUrl");
        }

        //Проверям то что пользователь не сможет оформить заказ, если его корзина пустая
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            
            CartController target = new CartController(null, mock.Object);

            ViewResult result = target.Checkout(cart, shippingDetails);

            //mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never); //Убедится что заказ не был передан в процесс отправки сообщения

            Assert.AreEqual("", result.ViewName);

            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        //Теперь проверяем, что пользотватель не может заказать товар, если он не правильно заполнил поля доставки
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Product p1 = new Product { Name = "p1" };
            Cart cart = new Cart();
            cart.AddItem(p1, 1);

            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(cart, new ShippingDetails());

            //mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, target.ViewData.ModelState.IsValid);
        }

        //Теперь проверям то, что заказ может быть успешно обработан
        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once);

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
