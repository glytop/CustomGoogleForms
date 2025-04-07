using CourseProject.Attributes;
using CourseProject.Data.Repositories;
using CourseProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class AdminController : Controller
    {
        private IUserRepositoryReal _userRepository;

        public AdminController(IUserRepositoryReal userRepository)
        {
            _userRepository = userRepository;
        }

        [IsAuthenticated]
        public IActionResult Activity()
        {
            var users = _userRepository.GetAll()
                .OrderByDescending(u => u.LastLoginTime)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    IsBlocked = u.IsBlocked,
                    LastLoginTime = u.LastLoginTime,
                    CreatedAt = u.CreatedAt
                }).ToList();

            return View(users);
        }

        [HttpPost]
        [IsAuthenticated]
        public IActionResult Block(List<Guid> id)
        {
            _userRepository.BlockUsers(id);
            TempData["SuccessMessage"] = "The user(s) has been successfully blocked!";
            return RedirectToAction("Activity");
        }

        [HttpPost]
        [IsAuthenticated]
        public IActionResult Unblock(List<Guid> id)
        {
            _userRepository.UnblockUsers(id);
            TempData["SuccessMessage"] = "The user(s) has been successfully unblocked!";
            return RedirectToAction("Activity");
        }

        [HttpPost]
        [IsAuthenticated]
        public IActionResult Delete(List<Guid> id)
        {
            _userRepository.DeleteUsers(id);
            TempData["SuccessMessage"] = "The user(s) has been successfully deleted!";
            return RedirectToAction("Activity");
        }
    }
}
