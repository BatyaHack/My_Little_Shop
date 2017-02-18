using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SportsStore.Models.Abstract;
using System.Web.Security;

namespace SportsStore.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool resultat = FormsAuthentication.Authenticate(username, password); //Authenticate - выполняем аундентификацию для стиля аундентификации forms-аутентификации
            if (resultat)
            {
                FormsAuthentication.SetAuthCookie(username, false); //Создаем куки, что бы пользователю не пришлось каждый раз логиниться (живут 48 часов)
            }

            return resultat;
        }
    }
    //Не забыть создать привязчу в Ninject


    //FormsAuthentication.Authenticate - я вляется устаревшим. Заменить на что то другое
}