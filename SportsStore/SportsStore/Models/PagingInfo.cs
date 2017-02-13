using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.Models
{
    // Это вспомогательный класс. Наши товары распалагаются на разных страницах.
    // И что бы осуществлять проход по этим старницам, мы должны создать этот дпольнительный класс
    // Котоырй будет содержать информации о страницах и обзем количестве товара в хранилище
    // Это называется моделью представления
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

    /*
    Модель представления не является частью доменной модели. Это просто удобный класс для
    передачи данных между представлением и контроллером. 

    Теперь нужно добавить вспомогательный метод в HTML
    */

    }
}