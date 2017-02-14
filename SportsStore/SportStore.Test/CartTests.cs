using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsStore.Models.Entities;
using System.Linq;

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
    }
}
