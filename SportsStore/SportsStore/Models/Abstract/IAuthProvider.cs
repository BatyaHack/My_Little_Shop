using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Models.Abstract
{
    // Так как для проверки аундентификации нам нужно вызываеть статические методы Authenticate и SetAuthCookie
    // А статические методы в методе действия могут ухуджить тестирование, так как Moq может создавать имитации только членов экземпляра

    /*
        Лучший способ решения этой проблемы - отделить контроллер от класса со статическими методами
    с помощью интерфейса. Дополнительное преимущество этого подхода заключается в том, что он
    вписывается в более широкий шаблон проектирования MVC и облегчит в дальнейшем переход на
    другую систему аутентификации.
     */
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password); //Реализация данного интерфейса в FormsAuthProvider
    }
}