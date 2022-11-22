using System.Linq.Expressions;

using CdPo.Common.Entity;

namespace CdPo.Model.Interfaces;

/// <summary>
/// Хранилище данных по сущности-наследника <see cref="BaseEntity"/>>
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public interface IGenericRepository<TEntity>
    where TEntity: BaseEntity, new()
{
    /// <summary>
    /// Получить все записи сущности <see cref="TEntity"/>
    /// </summary>
    /// <param name="wherePredicate">Условный предикат</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> wherePredicate = default, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить первую запись сущности <see cref="TEntity"/>
    /// </summary>
    /// <param name="wherePredicate">Условный предикат</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> wherePredicate = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить запись с указанным идентификатором
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<TEntity> GetAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить уже существующую запись сущности <see cref="TEntity"/>
    /// </summary>
    /// <param name="entity">Сущность <see cref="TEntity"/></param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новую запись сущности <see cref="TEntity"/>
    /// </summary>
    /// <param name="entity">Сущность <see cref="TEntity"/></param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить запись с указанным идентификатором
    /// </summary>
    /// <param name="entity">Сущность <see cref="TEntity"/></param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}