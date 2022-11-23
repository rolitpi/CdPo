using System.Linq.Expressions;

using CdPo.Common.Entity;
using CdPo.Model.Interfaces;
using CdPo.Web.DataAccess;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CdPo.Web.Services.Storage;

///<inheritdoc cref="IGenericRepository{TEntity}"/>
public class EfGenericRepository<TEntity>: IGenericRepository<TEntity>
    where TEntity: BaseEntity, new()
{
    /// <summary>
    /// Контекст бд
    /// </summary>
    protected readonly DataContext DataContext;

    /// <summary>
    /// Таблица сущностей <see cref="TEntity"/>
    /// </summary>
    protected readonly DbSet<TEntity> DbSet;

    public EfGenericRepository(DataContext dataContext)
    {
        DataContext = dataContext;
        DbSet = dataContext.Set<TEntity>();
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> wherePredicate = default,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        IQueryable<TEntity> query = DbSet;

        if (wherePredicate != default)
        {
            query = query.Where(wherePredicate);
        }

        if (includes != default)
        {
            query = includes.Aggregate(query, 
                (current, include) => current.Include(include));
        }

        return await query.ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> wherePredicate = default, CancellationToken cancellationToken = default)
        => wherePredicate != default
            ? DbSet.FirstOrDefaultAsync(wherePredicate, cancellationToken)
            : DbSet.FirstOrDefaultAsync(cancellationToken);

    ///<inheritdoc/>
    public async Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync(id);

    ///<inheritdoc/>
    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Entry(entity).State = EntityState.Modified;
        await DataContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    ///<inheritdoc/>
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Add(entity);
        await DataContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        await DataContext.SaveChangesAsync(cancellationToken);
    }
}