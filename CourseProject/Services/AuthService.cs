using CourseProject.Attributes;

namespace CourseProject.Services
{
    [AutoRegisterFlag]
    public class AuthService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public const string AUTH_TYPE_KEY = "Smile";
        public const string CLAIM_TYPE_ID = "Id";
        public const string CLAIM_TYPE_NAME = "Name";
        public const string CLAIM_TYPE_IS_BLOCKED = "IsBlocked";

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated()
        {
            return GetUserId() is not null;
        }

        public bool IsBlocked()
        {
            var isStr = GetClaimValue(CLAIM_TYPE_IS_BLOCKED);
            if (isStr is null)
            {
                return false;
            }

            return bool.Parse(isStr);
        }

        public string GetName()
        {
            return GetClaimValue(CLAIM_TYPE_NAME) ?? "Гость";
        }

        public Guid? GetUserId()
        {
            var isStr = GetClaimValue(CLAIM_TYPE_ID);
            if (isStr is null)
            {
                return null;
            }

            return Guid.Parse(isStr);
        }

        private string? GetClaimValue(string type)
        {
            return _httpContextAccessor
                .HttpContext!
               .User
               .Claims
               .FirstOrDefault(x => x.Type == type)
               ?.Value;
        }
    }
}
