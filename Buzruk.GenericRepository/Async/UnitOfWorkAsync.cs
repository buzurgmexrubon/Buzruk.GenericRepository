namespace Buzruk.GenericRepository;

public class UnitOfWorkAsync<DbContextClass>(DbContextClass context) :
  IUnitOfWorkAsync
  where DbContextClass : DbContext
{
  protected readonly DbContextClass AppDbContext = context;
  private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

  public virtual async Task<IGenericRepositoryAsync<T>> GetRepositoryAsync<T>(CancellationToken cancellationToken = default) where T : class
  {
    if (_repositories.TryGetValue(typeof(T), out object repository))
    {
      return (IGenericRepositoryAsync<T>)repository;
    }

    var newRepository = new GenericRepositoryAsync<DbContextClass, T>(AppDbContext);
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

    int changesSaved = await AppDbContext.SaveChangesAsync();

    if (afterSaveAction != null)
    {
      await afterSaveAction();
    }

    return changesSaved;
  }
}
