using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SportsStore.Models.Entities;

namespace SportsStore.Models
{
    // После того как мы сделали две модели Product и PagingInfo
    // нам нужно как то передавать эти две модели в представление
    // раньше мы передавали только Product в представление
    // и мы решили объеденить Product и PagingInfo (хотя PagingInfo можно было бы передавать через ViewBag - хотя это и не правильно)
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; } //класс Products
        public PagingInfo PagingInfo { get; set; } // класс PagingInfo

        //Теперь нум нужно обновить List что бы он передавал в модель ProductsListViewModel, а не Products   
        
        //Добавляем выбор по категориям
        public string CurrentCategory { get; set; }   
    }
}