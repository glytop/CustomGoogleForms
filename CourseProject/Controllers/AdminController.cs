using CourseProject.Attributes;
using CourseProject.Data.Repositories;
using CourseProject.Models;
using CourseProject.Services;
using Enums;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [IsAdmin]
    public class AdminController : Controller
    {
        private IUserRepositoryReal _userRepository;
        private EnumHelper _enumHelper;

        public AdminController(IUserRepositoryReal userRepository, EnumHelper enumHelper)
        {
            _userRepository = userRepository;
            _enumHelper = enumHelper;
        }

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
                    CreatedAt = u.CreatedAt,
                    Roles = _enumHelper.GetNames(u.Role),
                })
                .ToList();

            var viewModel = new AdminUserViewModel();
            viewModel.Users = users;

            viewModel.Roles = _enumHelper.GetSelectListItems<Role>();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Block(List<Guid> id)
        {
            _userRepository.BlockUsers(id);
            TempData["SuccessMessage"] = "The user(s) has been successfully blocked!";
            return RedirectToAction("Activity");
        }

        [HttpPost]
        public IActionResult Unblock(List<Guid> id)
        {
            _userRepository.UnblockUsers(id);
            TempData["SuccessMessage"] = "The user(s) has been successfully unblocked!";
            return RedirectToAction("Activity");
        }

        [HttpPost]
        public IActionResult Delete(List<Guid> id)
        {
            _userRepository.DeleteUsers(id);
            TempData["SuccessMessage"] = "The user(s) has been successfully deleted!";
            return RedirectToAction("Activity");
        }

        [HttpPost]
        public IActionResult UpdateRoles(Dictionary<Guid, List<Role>> updatedRoles)
        {
            _userRepository.UpdateRoles(updatedRoles);
            TempData["SuccessMessage"] = "The user(s) role(s) has been successfully updated!";
            return RedirectToAction("Activity");
        }
    }
}
