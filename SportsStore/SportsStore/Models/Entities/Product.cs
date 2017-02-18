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
        [HiddenInput(DisplayValue = false)] // Указываем, что когда мы будем вызывать Html.EditorForModle() в представлении то это поле не должно визуализироваться        
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Введите имя ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        [DataType(DataType.MultilineText)] // //Указываем, что когда мы будем вызывать Html.EditorForModle() в представлении то это поле должно визуализироваться как МультиБокс для текста
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Вы ввели неверную цену")] //Range - устанавливает минимальное и максимальное значение, которое может принять поле
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите категорию")]
        public string Category { get; set; }

        //После того как мы добавили возможность додавлять картинки в БД нам немобходимо добавить два новых поля

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMineType { get; set; }

        #region Совет по ImageData ImageMineType 
        //Мы не хотим, чтобы какое-либо из этих новых свойств было видимым при визуализизации
        //редактора.Для этого мы используем атрибут HiddenInput в свойстве ImageMimeType. Нам не нужно
        //ничего делать со свойством ImageData, потому что платформа не визуализирует редактор для
        //массивов байтов.Она делает это только для "простых" типов, таких как int, string, DateTime и так
        //далее.
        #endregion
    }

    //Атрибут HiddenInput сообщает MVC Framework, что свойство нужно визуализировать как скрытый
    //элемент формы, а атрибут DataType позволяет указать, как значение должно отображаться и
    //редактироваться.

}