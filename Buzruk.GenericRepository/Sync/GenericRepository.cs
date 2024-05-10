namespace Buzruk.GenericRepository;

public class GenericRepository<DbContextClass, T>(DbContextClass dbContext)
  : IGenericRepository<T>
  where DbContextClass : DbContext
  where T : class
{
  #region Fields

  /// <summary>
  /// The underlying DbContext instance used for accessing the database.
  /// </summary>
  protected readonly DbContextClass AppDbContext = dbContext;

  /// <summary>
  /// The DbSet object representing the collection of entities of type T in the DbContext.
  /// </summary>
  private readonly DbSet<T> _dbSet = dbContext.Set<T>();

  #endregion

  #region CRUD (Get, GetPaged, Add, AddRange, Update, UpdateRange, Remove, RemoveRange)

  public virtual T Get(
      Expression<Func<T, bool>> predicate,
      bool tracking = false)
  {
    IQueryable<T> query = _dbSet;

    if (!tracking)
    {
      query = query.AsNoTracking();
    }

    return query.FirstOrDefault(predicate);
  }

  public virtual IEnumerable<T> GetAll(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      bool tracking = false)
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

    return query.ToList();
  }

  public virtual PagedResults<T> GetPaged(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      int pageNumber = 1,
      int pageSize = int.MaxValue,
    bool tracking = false)
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

    int totalItemsCount = query.Count();
    int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

    var results = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

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

  public virtual void Add(T entity) => _dbSet.Add(entity);

  public virtual void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

  public virtual void Update(T entity) => _dbSet.Update(entity);

  public virtual void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

  public virtual void Remove(T entity) => _dbSet.Remove(entity);

  public virtual void Remove(Expression<Func<T, bool>> keyPredicate)
  {
    var entity = _dbSet.FirstOrDefault(keyPredicate);

    if (entity is null)
    {
      throw new ArgumentNullException(nameof(entity));
    }

    _dbSet.Remove(entity);
  }

  public virtual void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

  #endregion

  #region Other Methods (Exists, Count, LongCount, CountBy)

  public virtual bool Exists(Expression<Func<T, bool>> predicate) => _dbSet.Any(predicate);

  public virtual int Count(Expression<Func<T, bool>>? predicate = null)
  {
    if (predicate is null)
    {
      return _dbSet.Count();
    }

    return _dbSet.Count(predicate);
  }

  public virtual long LongCount(Expression<Func<T, bool>>? predicate = null)
  {
    if (predicate is null)
    {
      return _dbSet.LongCount();
    }

    return _dbSet.LongCount(predicate);
  }

  public virtual int CountBy<TProperty>(Expression<Func<T, TProperty>> groupExpression,
                                                   Expression<Func<T, bool>>? predicate = null)
  {
    if (predicate is null)
    {
      return _dbSet.GroupBy(groupExpression).Count();
    }
    
    return _dbSet.Where(predicate).GroupBy(groupExpression).Count();
  }

  #endregion
}
