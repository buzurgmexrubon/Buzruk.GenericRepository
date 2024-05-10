namespace Buzruk.GenericRepository;

public class UnitOfWork<DbContextClass>(DbContextClass context)
  : IUnitOfWork
  where DbContextClass : DbContext
{
  protected readonly DbContextClass AppDbContext = context;
  private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

  public IGenericRepository<T> GetRepository<T>() where T : class
  {
    if (_repositories.TryGetValue(typeof(T), out object repository))
    {
      return (IGenericRepository<T>)repository;
    }

    var newRepository = new GenericRepository<DbContextClass, T>(AppDbContext);
    _repositories.Add(typeof(T), newRepository);
    return newRepository;
  }

  public virtual int SaveChanges(Action? beforeSaveAction = null,
                                 Action? afterSaveAction = null)
  {
    if (beforeSaveAction != null)
    {
      beforeSaveAction();
    }

    int changesSaved = AppDbContext.SaveChanges();

    if (afterSaveAction != null)
    {
      afterSaveAction();
    }

    return changesSaved;
  }
}
