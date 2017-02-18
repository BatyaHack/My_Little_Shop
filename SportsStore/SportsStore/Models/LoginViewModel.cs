using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models
{
    public class LoginViewModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    /*Учитывая то, здесь только два свойства, можно было бы попытаться обойтись без модели и
использовать ViewBag для передачи данных в представление. Тем не менее, лучше определить
модель представления, чтобы данные, которые передаются от контроллера к представлению и от
механизма связывания в метод действия, были последовательно типизированы. Это облегчит
использование шаблонных вспомогательных методов представления
     */
}