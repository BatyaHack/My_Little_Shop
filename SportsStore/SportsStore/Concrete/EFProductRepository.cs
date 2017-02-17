using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Models.Entities;
using SportsStore.Models.Abstract;

namespace SportsStore.Concrete
{
    //А теперь реализуем полученние данных из БД не заглушкой, а реальными методами
    public class EFProductRepository : IProductsRepository
    {

        private EFDbContext context = new EFDbContext();

        public IQueryable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }

        //Реализация удаления
        public Product DeleteProduct(int productID)
        {
            Product dbdel = context.Products.Find(productID);
            if (dbdel != null)
            {
                context.Products.Remove(dbdel);
                context.SaveChanges();
            }

            return dbdel;
        }


        //Рализация сохранения
        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);

                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Price = product.Price;
                    dbEntry.Description = product.Description;
                    dbEntry.Category = product.Category;
                }
            }

            context.SaveChanges();
        }
    }
}

#region ВНИМАТЕЛЬНО ПРОЧИТАТЬ
//Это необходимо потому, что Entity Framework отслеживает объекты, которые он создает из базы
//данных.Объект, который передается в метод SaveChanges, создается MVC Framework с помощью
//стандартной модели связывания, что означает, что Entity Framework ничего не узнает об объекте
//параметра и не применит обновления к базе данных. Есть много путей решения этой проблемы, и мы
//выбрали самый простой: размещение соответствующего объекта, о котором будет знать Entity
//Framework, и явное его обновление.
//Альтернативный подход заключается в создании пользовательского механизма связывания, который
//будет только получать объекты из хранилища.Этот подход может показаться более гладким, но для
//него потребовалось бы добавить возможность поиска в интерфейс хранилища, чтобы мы могли бы
//найти объекты Product по значению ProductID.
//Недостаток такого подхода заключается в том, что мы начали бы добавлять функциональность к
//абстрактному определению хранилища для того, чтобы обойти ограничения конкретной реализации.
//Если мы будем переключаться между реализациями хранилища в будущем, есть риск того, что
//придется реализовать возможность поиска, для которой нет готовой поддержки в новой технологии
//хранения.Гибкость MVC Framework, как, к примеру, метод SaveProduct, может показаться хорошей
//возможностью, чтобы не использовать обходные пути, но она поставит под удар дизайн вашего
//приложения.
#endregion