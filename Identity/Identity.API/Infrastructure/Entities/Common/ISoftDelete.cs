namespace Identity.API.Infrastructure.Entities.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}