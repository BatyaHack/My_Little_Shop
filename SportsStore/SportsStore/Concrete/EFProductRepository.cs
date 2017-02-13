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
    }
}