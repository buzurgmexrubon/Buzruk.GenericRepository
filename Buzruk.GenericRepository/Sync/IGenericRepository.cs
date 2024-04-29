namespace Buzruk.GenericRepository;

/// <summary>
/// This interface defines a set of synchronous methods for generic repository implementations 
/// that manage entities of a specific type within a DbContext context.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public interface IGenericRepository<T> where T : class
{
  #region CRUD (Get, GetPaged, Add, AddRange, Update, UpdateRange, Remove, RemoveRange)

  /// <summary>
  /// Retrieves a filtered and optionally sorted collection of entities of type T.
  /// Supports eager loading of related entities through include and thenInclude delegates.
  /// </summary>
  /// <typeparam name="T">The type of entity being retrieved.</typeparam>
  /// <param name="predicates">An optional array of lambda expressions used for filtering. 
  /// The query will be filtered by applying all provided predicates with logical AND. (default: null)</param>
  /// <param name="orderBy">An optional lambda expression that specifies the sorting criteria for the results. (default: null)</param>
  /// <param name="include">An optional delegate function that specifies related entities to be included in the results. 
  /// This allows for eager loading of child entities in a single database call. (default: null)</param>
  /// <param name="thenInclude">An optional delegate function for nested eager loading of related entities within the already included entities. (default: null)</param>
  /// <param name="tracking">A flag indicating whether to track changes for the retrieved entities. Defaults to false.</param>
  /// <returns>An `IEnumerable<T>` containing the filtered and optionally sorted entities.</returns>
  public IEnumerable<T> Get(
    Expression<Func<T, bool>>[]? predicates = null,
    Expression<Func<T, object>>? orderBy = null,
    Func<IQueryable<T>, IQueryable<T>>? include = null,
    Func<IQueryable<T>, IQueryable<T>>? thenInclude = null,
    bool tracking = false);

  /// <summary>
  /// Retrieves a filtered and optionally sorted collection of entities of type T in a paged format.
  /// Supports eager loading of related entities through include and thenInclude delegates.
  /// </summary>
  /// <typeparam name="T">The type of entity being retrieved.</typeparam>
  /// <param name="predicates">An optional array of lambda expressions used for filtering. 
  /// The query will be filtered by applying all provided predicates with logical AND. (default: null)</param>
  /// <param name="orderBy">An optional lambda expression that specifies the sorting criteria for the results. (default: null)</param>
  /// <param name="include">An optional delegate function that specifies related entities to be included in the results. 
  /// This allows for eager loading of child entities in a single database call. (default: null)</param>
  /// <param name="thenInclude">An optional delegate function for nested eager loading of related entities within the already included entities. (default: null)</param>
  /// <param name="pageNumber">The desired page number (1-based). Defaults to 1.</param>
  /// <param name="pageSize">The number of items to retrieve per page. Defaults to 10.</param>
  /// <param name="tracking">A flag indicating whether to track changes for the retrieved entities. Defaults to false.</param>
  /// <returns>A `PagedResults<T>` object containing the filtered and optionally sorted entities 
  /// along with pagination information.</returns>
  public PagedResults<T> GetPaged(
    Expression<Func<T, bool>>[]? predicates = null,
    Expression<Func<T, object>>? orderBy = null,
    Func<IQueryable<T>, IQueryable<T>>? include = null,
    Func<IQueryable<T>, IQueryable<T>>? thenInclude = null,
    int pageNumber = 1,
    int pageSize = 10,
    bool tracking = false);

  /// <summary>
  /// Adds an entity of type T to the underlying data store synchronously.
  /// </summary>
  /// <param name="entity">The entity to be added.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The added entity of type T.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public T Add(T entity, bool saveChanges = true);

  /// <summary>
  /// Adds a collection of entities of type T to the underlying data store synchronously.
  /// </summary>
  /// <param name="entities">The collection of entities to be added.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public void AddRange(IEnumerable<T> entities, bool saveChanges = true);

  /// <summary>
  /// Updates an entity of type T in the underlying data store synchronously.
  /// </summary>
  /// <param name="entity">The entity with updated values.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The updated entity of type T.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public T Update(T entity, bool saveChanges = true);

  /// <summary>
  /// Updates a collection of entities of type T in the underlying data store synchronously.
  /// </summary>
  /// <param name="entities">The collection of entities with updated values.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public void UpdateRange(IEnumerable<T> entities, bool saveChanges = true);

  /// <summary>
  /// Removes an entity of type T from the underlying data store synchronously.
  /// </summary>
  /// <param name="entity">The entity to be removed.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The deleted entity of type T (if found), otherwise null.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity is null.</exception>
  public T Remove(T entity, bool saveChanges = true);

  /// <summary>
  /// Removes an entity of type T from the underlying data store synchronously based on a separate key expression.
  /// </summary>
  /// <param name="keyPredicate">A lambda expression that specifies the condition to identify the entity for deletion.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <returns>The deleted entity of type T (if found), otherwise null.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided keyPredicate is null.</exception>
  public T Remove(Expression<Func<T, bool>> keyPredicate, bool saveChanges = true);

  /// <summary>
  /// Removes a collection of entities of type T from the underlying data store synchronously.
  /// </summary>
  /// <param name="entities">The collection of entities to be deleted.</param>
  /// <param name="saveChanges">A flag indicating whether to save the changes to the database immediately. Defaults to true.</param>
  /// <exception cref="ArgumentNullException">Thrown if the provided entity collection is null.</exception>
  public void RemoveRange(IEnumerable<T> entities, bool saveChanges = true);

  #endregion

  #region Other Methods (Exists, Count, LongCount, CountBy, SaveChanges)

  /// <summary>
  /// Synchronously checks whether an entity of type T exists in the underlying data store based on a provided predicate expression.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for checking entity existence.</param>
  /// <returns>True if at least one entity matching the predicate exists, otherwise false.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided predicate expression is null.</exception>
  public bool Exists(Expression<Func<T, bool>> predicate);

  /// <summary>
  /// Synchronously counts the number of entities of type T in the underlying data store that optionally match a provided predicate expression.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for counting entities. (Optional)</param>
  /// <returns>The number of entities matching the predicate (or all entities if no predicate is provided).</returns>
  public int Count(Expression<Func<T, bool>>? predicate = null);

  /// <summary>
  /// Synchronously counts the number of entities of type T in the underlying data store that optionally match a provided predicate expression. 
  /// This method uses a long data type to handle potentially large entity sets that might exceed the capacity of an integer.
  /// </summary>
  /// <param name="predicate">A lambda expression that defines the condition for counting entities. (Optional)</param>
  /// <returns>The number of entities matching the predicate (or all entities if no predicate is provided) as a long value.</returns>
  public long LongCount(Expression<Func<T, bool>>? predicate = null);

  /// <summary>
  /// (Optional) Synchronously counts the number of entities of type T in the underlying data store grouped by a specified property. 
  /// </summary>
  /// <param name="groupExpression">A lambda expression that specifies the property to group by.</param>
  /// <param name="predicate">A lambda expression that defines the condition for filtering entities before counting. (Optional)</param>
  /// <returns>The number of entities within each group.</returns>
  /// <exception cref="ArgumentNullException">Thrown if the provided groupExpression is null.</exception>
  public int CountBy<TProperty>(Expression<Func<T, TProperty>> groupExpression, Expression<Func<T, bool>> predicate = null);

  /// <summary>
  /// Synchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional synchronous delegate that allows executing custom logic before saving changes. (Action)</param>
  /// <param name="afterSaveAction">An optional synchronous delegate that allows executing custom logic after saving changes. (Action)</param>
  /// <returns>The number of entities saved.</returns>
  public int SaveChanges(Action? beforeSaveAction = null, Action? afterSaveAction = null);

  #endregion
}
