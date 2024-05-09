namespace Buzruk.GenericRepository;

public interface IGenericRepositoryAsync<T> where T : class
{
  #region CRUD (GetAsync, GetPagedAsync, AddAsync, AddRangeAsync, UpdateAsync, UpdateRangeAsync, RemoveAsync, RemoveRangeAsync)

  Task<T> GetAsync(
      Expression<Func<T, bool>> predicate,
      bool tracking = false,
      CancellationToken cancellationToken = default);

  Task<IEnumerable<T>> GetAllAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      bool tracking = false,
      CancellationToken cancellationToken = default);

  Task<PagedResults<T>> GetPagedAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      int pageNumber = 1,
      int pageSize = int.MaxValue,
      bool tracking = false,
      CancellationToken cancellationToken = default);

  Task AddAsync(T entity, CancellationToken cancellationToken = default);

  Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

  void Update(T entity);

  void UpdateRange(IEnumerable<T> entities);

  void Remove(T entity);

  void Remove(Expression<Func<T, bool>> keyPredicate);

  void RemoveRange(IEnumerable<T> entities);

  #endregion

  #region Other Methods (ExistsAsync, CountAsync, LongCountAsync, CountByAsync)

  Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate,
                         CancellationToken cancellationToken = default);

  Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
                       CancellationToken cancellationToken = default);

  Task<long> LongCountAsync(Expression<Func<T, bool>>? predicate = null,
                            CancellationToken cancellationToken = default);

  Task<int> CountByAsync<TProperty>(Expression<Func<T, TProperty>> groupExpression,
                                    Expression<Func<T, bool>>? predicate = null,
                                    CancellationToken cancellationToken = default);

  #endregion
}
