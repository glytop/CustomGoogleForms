using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseProject.Models
{
    public class AdminUserViewModel
    {
        public List<UserViewModel> Users { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}
