namespace Buzruk.GenericRepository;

public class GenericRepositoryAsync<DbContextClass, T>(DbContextClass dbContext)
  : IGenericRepositoryAsync<T>
  where DbContextClass : DbContext
  where T : class
{
  #region Fields

  protected readonly DbContextClass DbContext = dbContext;

  private readonly DbSet<T> _dbSet = dbContext.Set<T>();

  #endregion

  #region CRUD (GetAsync, GetPagedAsync, AddAsync, AddRangeAsync, UpdateAsync, UpdateRangeAsync, RemoveAsync, RemoveRangeAsync)

  public virtual async Task<T> GetAsync(
      Expression<Func<T, bool>> predicate,
      bool tracking = false,
      CancellationToken cancellationToken = default)
  {
    IQueryable<T> query = _dbSet;

    if (!tracking)
    {
      query = query.AsNoTracking();
    }

    return await query.FirstOrDefaultAsync(predicate);
  }

  public virtual async Task<IEnumerable<T>> GetAllAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      bool tracking = false,
      CancellationToken cancellationToken = default)
  {
    IQueryable<T> query = _dbSet;

    foreach (var predicate in predicates)
    {
      query.Where(predicate);
    }

    if (orderBy is not null)
    {
      query = thenBy is not null ?
        query.OrderBy(orderBy).ThenBy(thenBy)
        :
        query.OrderBy(orderBy);
    }

    if (!tracking)
    {
      query = query.AsNoTracking();
    }

    return await query.ToListAsync();
  }

  public virtual async Task<PagedResults<T>> GetPagedAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      int pageNumber = 1,
      int pageSize = int.MaxValue,
      bool tracking = false,
      CancellationToken cancellationToken = default)
  {
    IQueryable<T> query = _dbSet;

    foreach (var predicate in predicates)
    {
      query.Where(predicate);
    }

    if (orderBy is not null)
    {
      query = thenBy is not null ?
        query.OrderBy(orderBy).ThenBy(thenBy)
        :
        query.OrderBy(orderBy);
    }

    if (!tracking)
    {
      query = query.AsNoTracking();
    }

    int totalItemsCount = await query.CountAsync();
    int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

    var results = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    return new PagedResults<T>
    {
      Items = results,
      TotalItemsCount = totalItemsCount,
      TotalPages = totalPages,
      PageSize = pageSize,
      PageNumber = pageNumber,
      HasPreviousPage = pageNumber > 1,
      HasNextPage = pageNumber < totalPages,
      IsFirstPage = pageNumber == 1,
      IsLastPage = pageNumber == totalPages,
      FirstItemOnPage = (pageNumber - 1) * pageSize + 1,
      LastItemOnPage = Math.Min(pageNumber * pageSize, totalItemsCount)
    };
  }

  public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    =>  await _dbSet.AddAsync(entity);

  public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    => await _dbSet.AddRangeAsync(entities);

  #endregion

  #region Other Methods (ExistsAsync, CountAsync, LongCountAsync, CountByAsync)

  public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate,
                                              CancellationToken cancellationToken = default)
    => await _dbSet.AnyAsync(predicate);

  public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,
                                            CancellationToken cancellationToken = default)
  {
    if (predicate is null)
    {
      return await _dbSet.CountAsync();
    }

    return await _dbSet.CountAsync(predicate);
  }

  public virtual async Task<long> LongCountAsync(Expression<Func<T, bool>>? predicate = null,
                                                 CancellationToken cancellationToken = default)
  {
    if (predicate is null)
    {
      return await _dbSet.LongCountAsync();
    }

    return await _dbSet.LongCountAsync(predicate);
  }

  public virtual async Task<int> CountByAsync<TProperty>(Expression<Func<T, TProperty>> groupExpression,
                                                         Expression<Func<T, bool>>? predicate = null,
                                                         CancellationToken cancellationToken = default)
  {
    if (predicate is null)
    {
      return await _dbSet.GroupBy(groupExpression).CountAsync();
    }

    return await _dbSet.Where(predicate).GroupBy(groupExpression).CountAsync();
  }

  #endregion
}
