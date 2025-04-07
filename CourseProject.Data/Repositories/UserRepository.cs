using CourseProject.Data.Models;
using CourseProject.Interface.Repositories;
using System.Data;

namespace CourseProject.Data.Repositories
{
    public interface IUserRepositoryReal : IUserRepository<UserData>
    {
        public void BlockUsers(List<Guid> id);
        public void UnblockUsers(List<Guid> id);
        public void DeleteUsers(List<Guid> id);
        UserData? Login(string email, string password);
        void Register(string name, string email, string password);
        bool CheckIsEmailAvailable(string email);


    }
    public class UserRepository : BaseRepository<UserData>, IUserRepositoryReal
    {
        public UserRepository(WebDbContext webDbContext) : base(webDbContext)
        {
        }

        public UserData? GetByEmail(string email)
        {
            return _webDbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public UserData? GetById(Guid id)
        {
            return _webDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public List<UserData> GetAll()
        {
            return _webDbContext
                .Users
                .ToList();
        }

        public void Add(UserData user)
        {
            throw new NotImplementedException("User method Register to create a new User");
        }

        public void Update(UserData user)
        {
            _webDbContext.Users.Update(user);
            _webDbContext.SaveChanges();
        }

        public void BlockUsers(List<Guid> id)
        {
            var users = _webDbContext.Users.Where(u => id.Contains(u.Id)).ToList();
            foreach (var user in users)
            {
                user.IsBlocked = true;
            }
            _webDbContext.SaveChanges();
        }

        public void UnblockUsers(List<Guid> id)
        {
            var users = _webDbContext.Users.Where(u => id.Contains(u.Id)).ToList();
            foreach (var user in users)
            {
                user.IsBlocked = false;
            }
            _webDbContext.SaveChanges();
        }

        public void DeleteUsers(List<Guid> id)
        {
            var users = _webDbContext.Users.Where(u => id.Contains(u.Id)).ToList();

            _webDbContext.Users.RemoveRange(users);
            _webDbContext.SaveChanges();
        }

        public UserData? Login(string email, string password)
        {
            return _dbSet.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public void Register(string name, string email, string password)
        {
            if (_dbSet.Any(x => x.Email == email))
            {
                throw new Exception();
            }

            var user = new UserData
            {
                Name = name,
                Email = email,
                Password = password,
                IsBlocked = false,
                LastLoginTime = DateTime.UtcNow
            };

            _dbSet.Add(user);
            _webDbContext.SaveChanges();
        }

        public bool CheckIsEmailAvailable(string email)
        {
            return !_dbSet.Any(x => x.Email == email);
        }
    }
}
