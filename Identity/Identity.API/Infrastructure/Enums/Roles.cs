using Identity.API.Infrastructure.Entities;

namespace Identity.API.Infrastructure.Enums;

public class Roles
{
    public static Role Admin = new()
    {
        RoleId = 1,
        CodeName = "Admin",
        NameAr = "مدير",
        NameEn = "Admin",
        DescriptionAr = "مدير الموقع",
        DescriptionEn = "Site Administrator",
    };

    public static Role Member = new()
    {
        RoleId = 2,
        CodeName = "Member",
        NameAr = "عضو",
        NameEn = "Member",
        DescriptionAr = "عضو",
        DescriptionEn = "Member",
    };

    public static List<Role> All = new() { Admin, Member };
}