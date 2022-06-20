using Identity.API.Infrastructure.Entities.Common;

namespace Identity.API.Infrastructure.Entities;

public class Role : IBaseEntity
{
    public int RoleId { get; set; }
    public string CodeName { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; } = "System";
    public bool IsDeleted { get; set; }
    public ICollection<User> Users { get; set; }

    public Role()
    {
        Users = new HashSet<User>();
    }
}