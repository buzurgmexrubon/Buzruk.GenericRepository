namespace Buzruk.GenericRepository;

/// <summary>
/// This internal class the logic for filtering, sorting, 
/// and eager loading of related entities.
/// It takes an IQueryable<T> object as input in the constructor.
/// It provides methods for applying filters, sorting, and includes asynchronously.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ActionsServiceAsync<T>(IQueryable<T> query)
{
  private IQueryable<T> _query = query;

  /// <summary>
  /// Asynchronously applies the provided filter predicates to the internal IQueryable object.
  /// </summary>
  /// <typeparam name="T">The type of entity being filtered.</typeparam>
  /// <param name="predicates">An optional array of lambda expressions used for filtering. 
  /// The query will be filtered by applying all provided predicates with logical AND.</param>
  /// <returns>An asynchronous task that returns an IQueryable object of type T representing the filtered query.</returns>
  public async Task<IQueryable<T>> ApplyFiltersAsync(Expression<Func<T, bool>>[]? predicates,
                                                     CancellationToken cancellationToken = default)
  {
    if (predicates is not null)
    {
      foreach (var predicate in predicates)
      {
        _query = _query.Where(predicate);
      }
    }

    return await Task.FromResult(_query);
  }

  /// <summary>
  /// Applies sorting to an existing IQueryable data source asynchronously.
  /// </summary>
  /// <typeparam name="T">The type of the data in the IQueryable source.</typeparam>
  /// <param name="orderBy">
  /// An optional expression specifying the property or field to sort the data by in ascending order.
  /// </param>
  /// <param name="thenBy">
  /// An optional expression specifying a secondary property or field for chained sorting 
  /// in ascending order after the primary sort defined by `orderBy`.
  /// </param>
  /// <returns>An IQueryable representing the sorted data source.</returns>
  public async Task<IQueryable<T>> ApplySortingAsync(Expression<Func<T, object>>? orderBy,
                                                     Expression<Func<T, object>>? thenBy,
                                                     CancellationToken cancellationToken = default)
  {
    if (orderBy is not null)
    {
      _query = thenBy is not null ?
        _query.OrderBy(orderBy).ThenBy(thenBy)
        :
        _query.OrderBy(orderBy);
    }

    return await Task.FromResult(_query);
  }

  /// <summary>
  /// Applies eager loading of related entities to an existing IQueryable data source asynchronously.
  /// </summary>
  /// <typeparam name="T">The type of the data in the IQueryable source.</typeparam>
  /// <param name="includes">
  /// An optional collection of delegate expressions specifying the navigation properties to eager load.
  /// Each delegate expression takes an IQueryable and returns a new IQueryable with the related data included.
  /// </param>
  /// <param name="thenInclude">
  /// An optional delegate expression specifying a navigation property to eager load *after* including the properties 
  /// specified in `includes`. This allows for chained eager loading of nested related entities.
  /// </param>
  /// <returns>An IQueryable representing the data source with included related entities.</returns>
  public async Task<IQueryable<T>> ApplyIncludesAsync(Func<IQueryable<T>, IQueryable<T>>[]? includes,
                                                      Func<IQueryable<T>, IQueryable<T>>? thenInclude,
                                                      CancellationToken cancellationToken = default)
  {
    if (includes is not null)
    {
      foreach (var include in includes)
      {
        _query = include(_query);
      }
    }

    if (thenInclude is not null)
    {
      _query = thenInclude(_query);
    }

    return await Task.FromResult(_query);
  }
}
