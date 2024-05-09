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
