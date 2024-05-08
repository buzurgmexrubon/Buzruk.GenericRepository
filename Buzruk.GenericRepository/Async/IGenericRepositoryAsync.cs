namespace Buzruk.GenericRepository;

/// <summary>
/// This interface defines a set of asynchronous methods for generic repository implementations 
/// that manage entities of a specific type within a DbContext context.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public interface IGenericRepositoryAsync<T> where T : class
{
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
  /// <param name="tracking">
  /// Optional flags specifying whether to track changes to the retrieved entities for future updates.
  /// Use `ChangeTracking.TrackChanges` for full change tracking, or specific tracking options like `ChangeTracking.PropertyChanges` for partial tracking.
  /// If not specified, no change tracking is applied. Defaults to false.
  /// </param>
  /// <returns>A task that returns an `IEnumerable<T>` containing the filtered, sorted, and eagerly loaded entities.</returns>
  public Task<IEnumerable<T>> GetAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
      Func<IQueryable<T>, IQueryable<T>>? thenInclude = null,
      bool tracking = false,
      CancellationToken cancellationToken = default);

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
  /// <param name="tracking">
  /// Optional flags specifying whether to track changes to the retrieved entities for future updates.
  /// Use `ChangeTracking.TrackChanges` for full change tracking, or specific tracking options like `ChangeTracking.PropertyChanges` for partial tracking.
  /// If not specified, no change tracking is applied. Defaults to false.
  /// </param>
  /// <returns>A task that returns a `PagedResults<T>` object containing the filtered, sorted, and eagerly loaded entities 
  /// along with pagination information.</returns>
  public Task<PagedResults<T>> GetPagedAsync(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      Func<IQueryable<T>, IQueryable<T>>[]? includes = null,
      Func<IQueryable<T>, IQueryable<T>>? thenInclude = null,
      int pageNumber = 1,
      int pageSize = int.MaxValue,
      bool tracking = false,
      CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds an entity of type T to the underlying data store asynchronously.
  /// </summary>
  /// <param name="entity">The entity to be added.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The added entity of type T.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public Task<T> AddAsync(T entity, 
                          bool saveChanges = true,
                          CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds a collection of entities of type T to the underlying data store asynchronously.
  /// </summary>
  /// <param name="entities">The collection of entities to be added.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>An awaitable task.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public Task AddRangeAsync(IEnumerable<T> entities, 
                            bool saveChanges = true,
                            CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an entity of type T in the underlying data store asynchronously.
  /// </summary>
  /// <param name="entity">The entity with updated values.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The updated entity of type T.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public Task<T> UpdateAsync(T entity, 
                             bool saveChanges = true, 
                             CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates a collection of entities of type T in the underlying data store asynchronously.
  /// </summary>
  /// <param name="entities">The collection of entities with updated values.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public Task UpdateRangeAsync(IEnumerable<T> entities, 
                               bool saveChanges = true, 
                               CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes an entity of type T from the underlying data store asynchronously.
  /// </summary>
  /// <param name="entity">The entity to be removec.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The removed entity of type T (if found), otherwise null.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public Task<T> RemoveAsync(T entity, 
                             bool saveChanges = true, 
                             CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes an entity of type T from the underlying data store asynchronously based on a separate key expression.
  /// </summary>
  /// <param name="keyPredicate">A lambda expression that specifies the condition to identify the entity for deletion.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The deleted entity of type T (if found), otherwise null.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided keyPredicate is null.</exception>
  public Task<T> RemoveAsync(Expression<Func<T, bool>> keyPredicate, 
                             bool saveChanges = true, 
                             CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes a collection of entities of type T from the underlying data store asynchronously.
  /// </summary>
  /// <param name="entities">The collection of entities to be removed.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public Task RemoveRangeAsync(IEnumerable<T> entities, 
                               bool saveChanges = true, 
                               CancellationToken cancellationToken = default);

  #endregion

  #region Other Methods (ExistsAsync, CountAsync, LongCountAsync, CountByAsync, SaveChangesAsync)

  /// <summary>
  /// Asynchronously checks whether an entity of type T exists in the underlying data store based on a provided predicate expression.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for checking entity existence.</param>
  /// <returns>A task that returns true if at least one entity matching the predicate exists, otherwise false.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided predicate expression is null.</exception>
  public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, 
                                CancellationToken cancellationToken = default);

  /// <summary>
  /// Asynchronously counts the number of entities of type T in the underlying data store that optionally match a provided predicate expression.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for counting entities. (Optional)</param>
  /// <returns>A task that returns the number of entities matching the predicate (or all entities if no predicate is provided).</returns>
  public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, 
                              CancellationToken cancellationToken = default);

  /// <summary>
  /// Asynchronously counts the number of entities of type T in the underlying data store that optionally match a provided predicate expression. 
  /// This method uses a long data type to handle potentially large entity sets that might exceed the capacity of an integer.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for counting entities. (Optional)</param>
  /// <returns>A task that returns the number of entities matching the predicate (or all entities if no predicate is provided) as a long value.</returns>
  public Task<long> LongCountAsync(Expression<Func<T, bool>>? predicate = null, 
                                   CancellationToken cancellationToken = default);

  /// <summary>
  /// (Optional) Asynchronously counts the number of entities of type T in the underlying data store grouped by a specified property. (Overload)
  /// </summary>
  /// <param name="groupExpression">A lambda expression that specifies the property to group by.</param>
  /// <param name="predicate">A lambda expression that defines the condition for filtering entities before counting. (Optional)</param>
  /// <returns>A task that returns the number of entities within each group.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided groupExpression is null.</exception>
  public Task<int> CountByAsync<TProperty>(Expression<Func<T, TProperty>> groupExpression, 
                                           Expression<Func<T, bool>>? predicate = null, 
                                           CancellationToken cancellationToken = default);

  /// <summary>
  /// Asynchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional asynchronous delegate that allows executing custom logic before saving changes. (Func&lt;Task&gt;)</param>
  /// <param name="afterSaveAction">An optional asynchronous delegate that allows executing custom logic after saving changes. (Func&lt;Task&gt;)</param>
  /// <returns>A task that returns the number of entities saved.</returns>
  public Task<int> SaveChangesAsync(Func<Task>? beforeSaveAction = null, 
                                    Func<Task>? afterSaveAction = null, 
                                    CancellationToken cancellationToken = default);

  #endregion
}
