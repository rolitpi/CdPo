using System.Linq.Expressions;

using CdPo.Common.Entity;
using CdPo.Model.Interfaces;
using CdPo.Web.DataAccess;

using Microsoft.EntityFrameworkCore;

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
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> wherePredicate = default, CancellationToken cancellationToken = default)
        => wherePredicate != default
            ? await DbSet.AsNoTracking().Where(wherePredicate).ToListAsync(cancellationToken)
            : await DbSet.AsNoTracking().ToListAsync(cancellationToken);

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

    public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate = default, 
        params Expression<Func<TEntity, object>>[] includeProperties) 
        => predicate != default
            ? Include(includeProperties).Where(predicate).ToList()
            : Include(includeProperties).ToList();

    private IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        => includeProperties
            .Aggregate(DbSet.AsNoTracking(), 
                (current, includeProperty) => current.Include(includeProperty));
}