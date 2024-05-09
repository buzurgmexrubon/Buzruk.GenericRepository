namespace Buzruk.GenericRepository.Sync;

public class UnitOfWork<DbContextClass>(DbContextClass context)
  : IUnitOfWork
  where DbContextClass : DbContext
{
  protected readonly DbContextClass DbContext = context;
  private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

  public IGenericRepository<T> GetRepository<T>() where T : class
  {
    if (_repositories.TryGetValue(typeof(T), out object repository))
    {
      return (IGenericRepository<T>)repository;
    }

    var newRepository = new GenericRepository<DbContextClass, T>(DbContext);
    _repositories.Add(typeof(T), newRepository);
    return newRepository;
  }

  /// <summary>
  /// Synchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional synchronous delegate that allows executing custom logic before saving changes. (Action)</param>
  /// <param name="afterSaveAction">An optional synchronous delegate that allows executing custom logic after saving changes. (Action)</param>
  /// <returns>The number of entities saved.</returns>
  public virtual int SaveChanges(Action? beforeSaveAction = null,
                                 Action? afterSaveAction = null)
  {
    if (beforeSaveAction != null)
    {
      beforeSaveAction();
    }

    int changesSaved = DbContext.SaveChanges();

    if (afterSaveAction != null)
    {
      afterSaveAction();
    }

    return changesSaved;
  }
}
