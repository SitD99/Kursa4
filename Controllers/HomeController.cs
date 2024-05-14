using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public IActionResult ChooseAction()
        {
            return View();
        }

        // POST-действие для обработки выбора пользователя
        [HttpPost]
        [Authorize]
        public IActionResult ChooseAction(string action)
        {
            if (action == "Create")
            {
                return RedirectToAction("Create", "CardSet");
            }
            else if (action == "View")
            {
                return RedirectToAction("Index", "CardSet");
            }
            else if (action == "Logout")
            {
                // Выполняем выход из системы
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "User");
            }
            else
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // Переход на страницу входа/регистрации
                return RedirectToAction("Login", "User");
            }
        }
    }
}
