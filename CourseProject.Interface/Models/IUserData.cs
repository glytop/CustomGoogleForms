namespace CourseProject.Interface.Models
{
    public interface IUserData : IBaseData
    {
        string Name { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        DateTime LastLoginTime { get; set; }
        bool IsBlocked { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
