using CourseProject.Interface.Models;
using Enums;

namespace CourseProject.Data.Models
{
    public class UserData : BaseData, IUserData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
