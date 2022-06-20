namespace Identity.API.Infrastructure.Entities.Common;

public interface IBaseEntity : ISoftDelete
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
}