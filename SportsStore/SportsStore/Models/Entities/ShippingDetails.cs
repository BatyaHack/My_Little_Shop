using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.Entities
{
    public class ShippingDetails
    {
        //Добавляем поля с валидацией

        [Required(ErrorMessage = "Вы не ввели имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Вы не ввели адрес")]
        public string Line1 { get; set; }

        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Вы не ввели город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Вы не ввели область")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "Вы не ввели область")]
        public string Country { get; set; }

        public bool GifWrap { get; set; }
    }
}