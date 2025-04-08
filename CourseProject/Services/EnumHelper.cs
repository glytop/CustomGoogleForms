using CourseProject.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseProject.Services
{
    [AutoRegisterFlag]
    public class EnumHelper
    {
        public List<string> GetNames<T>(T userRole)
            where T : Enum
        {
            var type = typeof(T);

            var names = type
                .GetEnumValues()
                .Cast<T>()
                .Where(r => userRole.HasFlag(r))
                .Select(r => type.GetEnumName(r))
                .ToList();

            return names;
        }

        public List<SelectListItem> GetSelectListItems<T>()
             where T : Enum
        {
            var type = typeof(T);
            return type.GetEnumValues()
                .Cast<T>()
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = type.GetEnumName(x)
                })
                .ToList();
        }
    }
}
