namespace Buzruk.GenericRepository;

public interface IUnitOfWork
{
  IGenericRepository<T> GetRepository<T>() where T : class;

  int SaveChanges(Action? beforeSaveAction = null, Action? afterSaveAction = null);
}
