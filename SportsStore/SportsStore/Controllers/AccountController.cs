using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider auntiti;
        public AccountController(IAuthProvider aut)
        {
            auntiti = aut;
        }
        
        // GET: Account
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login, string returnUrl) //для данного метода есть тест в AdminSecurityTests
        {
            if (ModelState.IsValid)
            {
                if (auntiti.Authenticate(login.LoginName, login.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin")); //если returnUrl = null, то возвращается Url.Action("Index", "Admin"). Если де не null то возвращается returnUrl
                }
                else
                {
                    ModelState.AddModelError("", "Вы ввели неверный логин или пароль");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}