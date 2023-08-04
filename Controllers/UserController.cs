using ALEXforums.Data;
using ALEXforums.Models.User;
using ALEXforums.Services.Hash;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;



namespace ALEXforums.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public UserController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }
        public IActionResult SignUp(UserSignUpModel? model)
        {
            if (HttpContext.Request.Method == "POST")   // є передані з форми дані
            {
                ViewData["form"] = _ValidateModel(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult SignIn([FromForm] String login, [FromForm] String password)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.Login == login);
            if (user != null)
            {
                if (user.PasswordHash == _hashService.HashString(password))
                {
                    // Автентифікацію пройдено
                    // зберігаємо у сесії id користувача
                    HttpContext.Session.SetString("AuthUserId", user.Id.ToString());
                    return Json(new { status = "OK" });
                }
            }
            return Json(new { status = "NO" });
        }
        private bool IsPasswordComplex(string password)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";

            return Regex.IsMatch(password, passwordPattern);
        }
        private bool IsLoginAlreadyUsed(string login)
        {
            bool loginExists = _dataContext.Users.Any(u => u.Login == login);

            return loginExists;
        }
        private String _ValidateModel(UserSignUpModel? model)
        {
            if (model == null) { return "Дані не передані"; }
            if (String.IsNullOrEmpty(model.Login)) { return "Логін не може бути порожним"; }
            if (IsLoginAlreadyUsed(model.Login))
            {
                return "Цей логін вже використовується.";
            }
            if (String.IsNullOrEmpty(model.Password)) { return "Пароль не може бути порожним"; }
            if (!IsPasswordComplex(model.Password))
            {
                return "Пароль не відповідає вимогам складності.пароль повинен містити принаймні одну малу літеру, одну велику літеру, одну цифру і має довжину не менше 8 символів.";
            }
            if (String.IsNullOrEmpty(model.RepeatPassword)) { return "Повтор паролю не може бути порожнім"; }
            if (model.Password != model.RepeatPassword) { return "Пароль та повтор паролю не співпадають"; }

            // завантажуємо файл-аватарку
            String? newName = null;
            if (model.AvatarFile != null)  // є файл
            {
                // Перевіряємо, чи дозволено тип файлу
                string[] allowedFileTypes = { ".jpg", ".jpeg", ".png" };
                string ext = Path.GetExtension(model.AvatarFile.FileName).ToLower();
                if (!allowedFileTypes.Contains(ext))
                {
                    return "Неприпустимий тип файлу для аватарки. Дозволені формати: JPG, JPEG, PNG.";
                }

                // Створюємо каталог, якщо він не існує
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Створюємо нове унікальне ім'я файлу
                newName = Guid.NewGuid().ToString() + ext;

                // Зберігаємо файл до каталогу завантажень
                string filePath = Path.Combine(uploadPath, newName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.AvatarFile.CopyTo(stream);
                }
            }
            else { newName = "11.jpg"; }

            // додаємо користувача до БД
            _dataContext.Users.Add(new Data.Entity.User
            {
                Id = Guid.NewGuid(),
                Login = model.Login,
                PasswordHash = _hashService.HashString(model.Password),
                Avatar = newName,
                RegisteredDt = DateTime.Now,
            });
            // зберігаємо внесені зміни
            _dataContext.SaveChanges();  // PlanetScale не підтримує асинхронні запити

            return String.Empty;
        }
        [HttpPost]

        public IActionResult SignOut(UserSignUpModel? model)

        {
            HttpContext.Session.Remove("AuthUserId"); // видалення ключа з сесії
            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}

