using CourseProject.Data.Repositories;
using CourseProject.Models;
using CourseProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseProject.Controllers
{
    public class AuthController : Controller
    {
        private IUserRepositoryReal _userRepository;

        public AuthController(IUserRepositoryReal userRepositoryReal)
        {
            _userRepository = userRepositoryReal;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            var user = _userRepository.Login(viewModel.Email, viewModel.Password);

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "The data is not valid";
                return View(viewModel);
            }

            if (user is null)
            {
                TempData["ErrorMessage"] = "Invalid email or password";
                return View(viewModel);
            }

            if (user.IsBlocked)
            {
                TempData["ErrorMessage"] = "The user is blocked";
                return View(viewModel);
            }

            user.LastLoginTime = DateTime.UtcNow;
            _userRepository.Update(user);

            var claims = new List<Claim>
            {
                new Claim(AuthService.CLAIM_TYPE_ID, user.Id.ToString()),
                new Claim(AuthService.CLAIM_TYPE_NAME, user.Name),
                new Claim(AuthService.CLAIM_TYPE_IS_BLOCKED, user.IsBlocked.ToString()),
                new Claim(AuthService.CLAIM_TYPE_ROLE, ((int)user.Role).ToString()),
                new Claim (ClaimTypes.AuthenticationMethod, AuthService.AUTH_TYPE_KEY),

            };

            var identity = new ClaimsIdentity(claims, AuthService.AUTH_TYPE_KEY);

            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(AuthService.AUTH_TYPE_KEY, principal,
                new AuthenticationProperties
                {
                    IsPersistent = viewModel.RememberMe
                }).Wait();

            TempData["SuccessMessage"] = "Successful authorization";

            return RedirectToAction("Activity", "Table");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid || (viewModel.Password != viewModel.ConfirmPassword)
                || (!_userRepository.CheckIsEmailAvailable(viewModel.Email)))
            {
                TempData["ErrorMessage"] = "Registration error";
                return View(viewModel);
            }

            _userRepository.Register(viewModel.Name, viewModel.Email, viewModel.Password);

            TempData["SuccessMessage"] = "Successful registration";

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext
                .SignOutAsync(AuthService.AUTH_TYPE_KEY)
                .Wait();
            return RedirectToAction("Login");
        }
    }
}
