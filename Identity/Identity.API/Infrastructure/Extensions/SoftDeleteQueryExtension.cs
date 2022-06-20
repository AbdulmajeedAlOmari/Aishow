using System.Linq.Expressions;
using System.Reflection;
using Identity.API.Infrastructure.Entities.Common;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Identity.API.Infrastructure.Extensions;

public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtension)
            .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)
            ?.MakeGenericMethod(entityData.ClrType);
        var filter = methodToCall?.Invoke(null, new object[] { });
        entityData.SetQueryFilter((LambdaExpression)filter!);
        entityData.AddIndex(entityData.
            FindProperty(nameof(ISoftDelete.IsDeleted)) ?? throw new InvalidOperationException());
    }
 
    private static LambdaExpression GetSoftDeleteFilter<TEntity>()
        where TEntity : class, ISoftDelete
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}