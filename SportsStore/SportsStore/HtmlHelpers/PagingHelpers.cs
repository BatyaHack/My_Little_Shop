using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Models;
using System.Text;

namespace SportsStore.HtmlHelpers
{
    // Вспомогательный метод, который будет генерировать HTML код для набора ссылок на страницу  нформацию, предоставленную в объекте PagingInfo(из модели)
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagininfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= pagininfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                if (i == pagininfo.CurrentPage)
                    tag.AddCssClass("selected");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());

            /*
             *  Метод расширения PageLinks генерирует HTML для набора ссылок на страницы, используя
                информацию, предоставленную в объекте PagingInfo. Параметр Func предоставляет возможность
                передачи делегата, который будет использоваться для генерации ссылок на другие страницы.

                Такаже для этого метода есть специальный тест

                + этот метод расширения(его пространство имне) нужно добавить в Views/Web.config что бы он стал доступный или же в само представление через @using
             */
        }
    }
}