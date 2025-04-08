using CourseProject.Data.Repositories;
using Enums;
using Microsoft.Extensions.DependencyInjection;

namespace CourseProject.Data
{
    public class Seed
    {
        public void Fill(IServiceProvider service)
        {
            using var di = service.CreateScope();
            UserFill(di);
        }

        private void UserFill(IServiceScope di)
        {
            var userRepository = di.ServiceProvider.GetRequiredService<IUserRepositoryReal>();
            if (!userRepository.IsAdminExist())
            {
                userRepository.Register("admin", "admin@admin.com", "admin", Role.Admin);
            }
        }
    }
}
