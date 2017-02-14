using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Models.Entities
{
    // Данная модель представляет себой корзину магазина
    // В Entities мы добавляем те модели, которые являются частью бизне слогики приложения (такие как Product и Cart)
    // PagingInfo и ProductList... являются всего лишь вспомогательными классами и не вы носятся в Entities
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            //проверям содержится ли в корзине продукт, который мы хотим добавить
            CartLine line = lineCollection.Where(p => p.Product.ProductID == product.ProductID).FirstOrDefault();
            //Если такого продукта нет, то добавляем его
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else //если такой продукт в корзине уже есть, то увеличивем его количество
            {
                line.Quantity += quantity;
            }
        }
        //Удаление элементов
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        //Посчитать сумму
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        //Полность очистить корзину
        public void Clear()
        {
            lineCollection.Clear();
        }

        //Возвращаем список lineCollection, так как он является private
        public IEnumerable<CartLine> Lines
        { get
            { return lineCollection; }
        }


        //Класс корзины
        public class CartLine
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }

        //ДЛЯ ДАННОГО КЛАССА ЕСТЬ ДВА ТЕСТА
    }
}