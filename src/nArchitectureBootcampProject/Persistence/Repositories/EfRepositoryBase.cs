using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using NArchitecture.Core.Persistence.Repositories;
using System.Reflection;

namespace Persistence.Repositories;
public abstract class EfRepositoryBase<TEntity, TEntityId, TContext> : NArchitecture.Core.Persistence.Repositories.EfRepositoryBase<TEntity, TEntityId, TContext> 
    where TEntity : Entity<TEntityId> 
    where TContext : DbContext
{
    public EfRepositoryBase(TContext context) : base(context)
    {
    }

    public new async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        await SoftDeleteInSpecificTableAsync(entity);

        //await SetEntityAsDeletedAsync(entity, permanent);

        await Context.SaveChangesAsync();

        return entity;
    }

    // To Soft Delete data from a single table
    protected async Task SoftDeleteInSpecificTableAsync(TEntity entity)
    {
        // Check if entity is soft deleted
        if (entity.DeletedDate.HasValue)
        {
            return;
        }

        entity.DeletedDate = DateTime.UtcNow;
        Context.Update(entity);
    }

    // To Soft Delete data-bound data from all relevant tables
    protected new async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            Context.Remove(entity);
        }
    }

    protected new async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (TEntity entity in entities)
        {
            await SetEntityAsDeletedAsync(entity, permanent);
        }
    }

    protected new void CheckHasEntityOneToOneRelation(TEntity entity)
    {
        // Check if the entity has one-to-one relationships
        TEntity entity2 = entity;
        IEnumerable<IForeignKey> foreignKeys = Context.Entry(entity2).Metadata.GetForeignKeys();

        if (foreignKeys.Any() && foreignKeys.All(x =>
        {
            if (x.DependentToPrincipal != null && !x.DependentToPrincipal.IsCollection)
            {
                return true;
            }
            if (x.PrincipalToDependent != null && !x.PrincipalToDependent.IsCollection)
            {
                return true;
            }
            return x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity2.GetType();
        }))
        {
            //You might want to throw an exception here.
             throw new InvalidOperationException("Entity has one-to-one relationship.");
        }
    }

    protected new IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        // Get the query provider type
        Type queryProviderType = query.Provider.GetType();

        // Find the CreateQuery<TElement> method using reflection
        MethodInfo createQueryMethod = queryProviderType
            .GetMethods()
            .FirstOrDefault(m => m.Name == nameof(query.Provider.CreateQuery) && m.IsGenericMethod)
            ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");

        // Invoke the CreateQuery<TElement> method to create a new IQueryable<object>
        var queryProviderQuery = (IQueryable<object>)createQueryMethod.Invoke(query.Provider, new object[] { query.Expression });

        // Filter out soft-deleted entities
        return queryProviderQuery.Where(x => !((IEntityTimestamps)x).DeletedDate.HasValue);
    }

    private new async Task SetEntityAsSoftDeletedAsync(IEntityTimestamps entity)
    {
        if (entity.DeletedDate.HasValue)
        {
            return;
        }

        entity.DeletedDate = DateTime.UtcNow;
        List<NavigationEntry> navigationsToHandle = Context.Entry(entity).Navigations.ToList();

        foreach (NavigationEntry navigation in navigationsToHandle)
        {
            if (!navigation.IsLoaded)
            {
                await navigation.LoadAsync();
            }

            if (navigation.CurrentValue is IEntityTimestamps relatedEntity)
            {
                await SetEntityAsSoftDeletedAsync(relatedEntity);
            }
            else if (navigation.CurrentValue is IEnumerable<IEntityTimestamps> relatedEntities)
            {
                foreach (IEntityTimestamps item in relatedEntities)
                {
                    await SetEntityAsSoftDeletedAsync(item);
                }
            }
        }

        Context.Update(entity);
    }
}