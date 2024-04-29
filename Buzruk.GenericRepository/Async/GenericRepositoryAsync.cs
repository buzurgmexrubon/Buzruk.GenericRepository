namespace Buzruk.GenericRepository;

/// <summary>
/// This abstract class provides a base implementation for generic asynchronous repositories 
/// that manage entities of a specific type within a DbContext context.
/// </summary>
/// <typeparam name="DbContextClass">The type of the DbContext class used for database access.</typeparam>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
/// <remarks>
/// This class provides a set of commonly used methods for CRUD (Create, Read, Update, Delete) operations 
/// and other entity management functionalities in an asynchronous manner. 
/// It leverages Entity Framework Core features for interacting with the database.
/// </remarks>
public abstract class GenericRepositoryAsync<DbContextClass, T>(DbContextClass dbContext)
  : IGenericRepositoryAsync<T>
  where DbContextClass : DbContext
  where T : class
{
  #region Fields

  /// <summary>
  /// The underlying DbContext instance used for accessing the database.
  /// </summary>
  protected readonly DbContextClass DbContext = dbContext;

  /// <summary>
  /// The DbSet object representing the collection of entities of type T in the DbContext.
  /// </summary>
  private readonly DbSet<T> _dbSet = dbContext.Set<T>();

  #endregion

  #region CRUD (GetAsync, GetPagedAsync, AddAsync, AddRangeAsync, UpdateAsync, UpdateRangeAsync, RemoveAsync, RemoveRangeAsync)

  /// <summary>
  /// Asynchronously retrieves a filtered, optionally sorted, and eagerly loaded collection of entities of type T.
  /// </summary>
  /// <typeparam name="T">The type of entity being retrieved.</typeparam>
  /// <param name="predicates">
  /// An optional collection of `Expression<Func<T, bool>>` delegates used to filter the retrieved data.
  /// Each predicate represents a filtering condition. Only items that match all specified predicates will be included in the results.
  /// The query will be filtered by applying all provided predicates with logical AND. (default: null)
  /// </param>
  /// <param name="orderBy">
  /// An optional expression specifying the property or field to sort the retrieved data by.
  /// You can use ascending or descending order using syntax like `x => x.PropertyName ASC` or `x => x.PropertyName DESC`.
  /// If not specified, no sorting is applied. (default: null)
  /// </param>
  /// <param name="thenBy">
  /// An optional expression specifying a secondary property or field for chained sorting 
  /// in ascending order after the primary sort defined by `orderBy`. (default: null)
  /// </param>
  /// <param name="includes">
  /// An optional collection of delegate expressions specifying the navigation properties to eager load for the main entity type.
  /// Each delegate expression takes an IQueryable and returns a new IQueryable with the related data included.
  /// Use methods like `q => q.Include(x => x.PropertyName)` to construct the expressions. (default: null)
  /// </param>
  /// <param name="thenInclude">
  /// An optional delegate expression specifying a navigation property to eager load *after* including the properties 
  /// specified in `includes`. This allows for chained eager loading of nested related entities. (default: null)
  /// </param>  
  /// <param name="tracking">A flag indicating whether to track changes for the retrieved entities. Defaults to false.</param>
  /// <returns>A task that returns an `IEnumerable<T>` containing the filtered, sorted, and eagerly loaded entities.</returns>
  public virtual async Task<IEnumerable<T>> GetAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
      Func<IQueryable<T>, IQueryable<T>>? thenInclude = null,
      bool tracking = false)
  {
      var service = new ActionsServiceAsync<T>(_dbSet);
      //var query = await service.ApplyFiltersAsync(predicates);
      //query = await service.ApplyIncludesAsync(includes, thenInclude);
      //query = await service.ApplySortingAsync(orderBy, thenBy);
      await service.ApplyFiltersAsync(predicates);
      await service.ApplyIncludesAsync(includes, thenInclude);
      var query = await service.ApplySortingAsync(orderBy, thenBy);
  
      if (!tracking)
      {
          query = query.AsNoTracking();
      }
  
      return await query.ToListAsync();
  }

  /// <summary>
  /// Asynchronously retrieves a filtered, optionally sorted, and eagerly loaded collection of entities of type T in a paged format.
  /// Supports eager loading of related entities through include and thenInclude delegates.
  /// </summary>
  /// <typeparam name="T">The type of entity being retrieved.</typeparam>
  /// <param name="predicates">
  /// An optional collection of `Expression<Func<T, bool>>` delegates used to filter the retrieved data.
  /// Each predicate represents a filtering condition. Only items that match all specified predicates will be included in the results.
  /// The query will be filtered by applying all provided predicates with logical AND. (default: null)
  /// </param>
  /// <param name="orderBy">
  /// An optional expression specifying the property or field to sort the retrieved data by.
  /// You can use ascending or descending order using syntax like `x => x.PropertyName ASC` or `x => x.PropertyName DESC`.
  /// If not specified, no sorting is applied. (default: null)
  /// </param>
  /// <param name="thenBy">
  /// An optional expression specifying a secondary property or field for chained sorting 
  /// in ascending order after the primary sort defined by `orderBy`. (default: null)
  /// </param>
  /// <param name="includes">
  /// An optional collection of delegate expressions specifying the navigation properties to eager load for the main entity type.
  /// Each delegate expression takes an IQueryable and returns a new IQueryable with the related data included.
  /// Use methods like `q => q.Include(x => x.PropertyName)` to construct the expressions. (default: null)
  /// </param>
  /// <param name="thenInclude">
  /// An optional delegate expression specifying a navigation property to eager load *after* including the properties 
  /// specified in `includes`. This allows for chained eager loading of nested related entities. (default: null)
  /// </param> 
  /// <param name="pageNumber">
  /// The zero-based page number of the data to retrieve. This specifies which page of results 
  /// you want to retrieve. Page numbering starts at 1, where 1 represents the first page. (default: 1)
  /// </param>
  /// <param name="pageSize">
  /// The number of items to retrieve per page. 
  /// If not specified, defaults to the maximum value for an integer (`int.MaxValue`), 
  /// effectively retrieving all data in a single page. 
  /// Consider using a reasonable page size to optimize performance 
  /// and avoid large data transfers. (default: int.MaxValue)
  /// </param>
  /// <param name="tracking">A flag indicating whether to track changes for the retrieved entities. Defaults to false.</param>
  /// <returns>A task that returns a `PagedResults<T>` object containing the filtered, sorted, and eagerly loaded entities 
  /// along with pagination information.</returns>
  public virtual async Task<PagedResults<T>> GetPagedAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
      Func<IQueryable<T>, IQueryable<T>>? thenInclude = null,
      int pageNumber = 1,
      int pageSize = int.MaxValue,
      bool tracking = false)
  {
    var service = new ActionsServiceAsync<T>(_dbSet);
      await service.ApplyFiltersAsync(predicates);
      await service.ApplyIncludesAsync(includes, thenInclude);
      var query = await service.ApplySortingAsync(orderBy, thenBy);

    int totalItemsCount = await query.CountAsync();
    int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

    if (!tracking)
    {
      query = query.AsNoTracking();
    }

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

  /// <summary>
  /// Adds an entity of type T to the underlying data store asynchronously.
  /// </summary>
  /// <param name="entity">The entity to be added.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The added entity of type T.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public virtual async Task<T> AddAsync(T entity, bool saveChanges = true)
  {
    if (entity is null)
    {
      throw new ArgumentNullException(nameof(entity));
    }

    _dbSet.Add(entity);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }

    return entity;
  }

  /// <summary>
  /// Adds a collection of entities of type T to the underlying data store asynchronously.
  /// </summary>
  /// <param name="entities">The collection of entities to be added.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>An awaitable task.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public virtual async Task AddRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
  {
    if (entities is null)
    {
      throw new ArgumentNullException(nameof(entities));
    }

    _dbSet.AddRange(entities);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }
  }

  /// <summary>
  /// Updates an entity of type T in the underlying data store asynchronously.
  /// </summary>
  /// <param name="entity">The entity with updated values.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The updated entity of type T.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public virtual async Task<T> UpdateAsync(T entity, bool saveChanges = true)
  {
    if (entity is null)
    {
      throw new ArgumentNullException(nameof(entity));
    }

    _dbSet.Update(entity);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }

    return entity;
  }

  /// <summary>
  /// Updates a collection of entities of type T in the underlying data store asynchronously.
  /// </summary>
  /// <param name="entities">The collection of entities with updated values.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
  {
    if (entities is null)
    {
      throw new ArgumentNullException(nameof(entities));
    }

    _dbSet.UpdateRange(entities);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }
  }

  /// <summary>
  /// Removes an entity of type T from the underlying data store asynchronously.
  /// </summary>
  /// <param name="entity">The entity to be removed.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The removed entity of type T (if found), otherwise null.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public virtual async Task<T> RemoveAsync(T entity, bool saveChanges = true)
  {
    if (entity is null)
    {
      throw new ArgumentNullException(nameof(entity));
    }

    _dbSet.Remove(entity);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }

    return entity;
  }

  /// <summary>
  /// Removes an entity of type T from the underlying data store asynchronously based on a separate key expression.
  /// </summary>
  /// <param name="keyPredicate">A lambda expression that specifies the condition to identify the entity for deletion.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The deleted entity of type T (if found), otherwise null.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided keyPredicate is null.</exception>
  public virtual async Task<T> RemoveAsync(Expression<Func<T, bool>> keyPredicate, bool saveChanges = true)
  {
    if (keyPredicate is null)
    {
      throw new ArgumentNullException(nameof(keyPredicate));
    }

    var entity = await _dbSet.FirstOrDefaultAsync(keyPredicate);

    if (entity is null)
    {
      return null; // Entity not found for deletion
    }

    _dbSet.Remove(entity);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }

    return entity;
  }

  /// <summary>
  /// Removes a collection of entities of type T from the underlying data store asynchronously.
  /// </summary>
  /// <param name="entities">The collection of entities to be removed.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public virtual async Task RemoveRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
  {
    if (entities is null)
    {
      throw new ArgumentNullException(nameof(entities));
    }

    _dbSet.RemoveRange(entities);

    if (saveChanges)
    {
      await DbContext.SaveChangesAsync();
    }
  }

  #endregion

  #region Other Methods (ExistsAsync, CountAsync, LongCountAsync, CountByAsync, SaveChangesAsync)

  /// <summary>
  /// Asynchronously checks whether an entity of type T exists in the underlying data store based on a provided predicate expression.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for checking entity existence.</param>
  /// <returns>A task that returns true if at least one entity matching the predicate exists, otherwise false.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided predicate expression is null.</exception>
  public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
  {
    if (predicate is null)
    {
      throw new ArgumentNullException(nameof(predicate));
    }

    return await _dbSet.AnyAsync(predicate);
  }

  /// <summary>
  /// Asynchronously counts the number of entities of type T in the underlying data store that optionally match a provided predicate expression.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for counting entities. (Optional)</param>
  /// <returns>A task that returns the number of entities matching the predicate (or all entities if no predicate is provided).</returns>
  public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
  { 
    if (predicate is null)
    {
      return await _dbSet.CountAsync();
    }
    else
    {
      return await _dbSet.CountAsync(predicate);
    }
  }

  /// <summary>
  /// Asynchronously counts the number of entities of type T in the underlying data store that optionally match a provided predicate expression. 
  /// This method uses a long data type to handle potentially large entity sets that might exceed the capacity of an integer.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for counting entities. (Optional)</param>
  /// <returns>A task that returns the number of entities matching the predicate (or all entities if no predicate is provided) as a long value.</returns>
  public virtual async Task<long> LongCountAsync(Expression<Func<T, bool>>? predicate = null)
  { 
    if (predicate is null)
    {
      return await _dbSet.LongCountAsync(); 
    }
    else
    {
      return await _dbSet.LongCountAsync(predicate); 
    }
  }

  /// <summary>
  /// (Optional) Asynchronously counts the number of entities of type T in the underlying data store grouped by a specified property. (Overload)
  /// </summary>
  /// <param name="groupExpression">A lambda expression that specifies the property to group by.</param>
  /// <param name="predicate">A lambda expression that defines the condition for filtering entities before counting. (Optional)</param>
  /// <returns>A task that returns the number of entities within each group.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided groupExpression is null.</exception>
  public virtual async Task<int> CountByAsync<TProperty>(Expression<Func<T, TProperty>> groupExpression, 
                                                         Expression<Func<T, bool>>? predicate = null)
  {
    if (groupExpression is null)
    {
      throw new ArgumentNullException(nameof(groupExpression));
    }

    if (predicate is null)
    {
      return await _dbSet.GroupBy(groupExpression).CountAsync();
    }
    else
    {
      return await _dbSet.Where(predicate).GroupBy(groupExpression).CountAsync();
    }
  }

  /// <summary>
  /// Asynchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional asynchronous delegate that allows executing custom logic before saving changes. (Func&lt;Task&gt;)</param>
  /// <param name="afterSaveAction">An optional asynchronous delegate that allows executing custom logic after saving changes. (Func&lt;Task&gt;)</param>
  /// <returns>A task that returns the number of entities saved.</returns>
  public virtual async Task<int> SaveChangesAsync(Func<Task>? beforeSaveAction = null, 
                                                  Func<Task>? afterSaveAction = null)
  {
    if (beforeSaveAction != null)
    {
      await beforeSaveAction();
    }

    int changesSaved = await DbContext.SaveChangesAsync();

    if (afterSaveAction != null)
    {
      await afterSaveAction();
    }

    return changesSaved;
  }

  #endregion
}
