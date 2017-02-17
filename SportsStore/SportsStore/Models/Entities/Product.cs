using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Models.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue =false)] // Указываем, что когда мы будем вызывать Html.EditorForModle() в представлении то это поле не должно визуализироваться        
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Введите имя ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        [DataType(DataType.MultilineText)] // //Указываем, что когда мы будем вызывать Html.EditorForModle() в представлении то это поле должно визуализироваться как МультиБокс для текста
        public string Description { get; set; }

        [Required]
        [Range(0.01 , double.MaxValue, ErrorMessage = "Вы ввели неверную цену")] //Range - устанавливает минимальное и максимальное значение, которое может принять поле
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите категорию")]
        public string Category { get; set; }
    }

        //Атрибут HiddenInput сообщает MVC Framework, что свойство нужно визуализировать как скрытый
        //элемент формы, а атрибут DataType позволяет указать, как значение должно отображаться и
        //редактироваться.

}