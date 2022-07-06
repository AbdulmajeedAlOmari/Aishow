namespace Common.API.Models.Entities;

public class CommonUserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public ICollection<CommonRoleDto> Roles { get; set; }
}

public class CommonRoleDto
{
    public int RoleId { get; set; }
    public string CodeName { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }
}