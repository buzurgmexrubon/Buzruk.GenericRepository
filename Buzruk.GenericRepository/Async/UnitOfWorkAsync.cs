namespace Buzruk.GenericRepository.Async;

public class UnitOfWorkAsync<DbContextClass>(DbContextClass context) :
  IUnitOfWorkAsync
  where DbContextClass : DbContext
{
  protected readonly DbContextClass DbContext = context;
  private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

  public virtual async Task<IGenericRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class
  {
    if (_repositories.TryGetValue(typeof(T), out object repository))
    {
      return (IGenericRepositoryAsync<T>)repository;
    }

    var newRepository = new GenericRepositoryAsync<DbContextClass, T>(DbContext);
    _repositories.Add(typeof(T), newRepository);
    return newRepository;
  }


  /// <summary>
  /// Asynchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional asynchronous delegate that allows executing custom logic before saving changes. (Func&lt;Task&gt;)</param>
  /// <param name="afterSaveAction">An optional asynchronous delegate that allows executing custom logic after saving changes. (Func&lt;Task&gt;)</param>
  /// <returns>A task that returns the number of entities saved.</returns>
  public virtual async Task<int> SaveChangesAsync(Func<Task>? beforeSaveAction = null,
                                                  Func<Task>? afterSaveAction = null,
                                                  CancellationToken cancellationToken = default)
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
}
