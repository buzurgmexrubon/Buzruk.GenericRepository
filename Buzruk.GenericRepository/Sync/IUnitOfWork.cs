namespace Buzruk.GenericRepository.Sync;

public interface IUnitOfWork
{
  IGenericRepository<T> GetRepository<T>() where T : class;

  /// <summary>
  /// Synchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional synchronous delegate that allows executing custom logic before saving changes. (Action)</param>
  /// <param name="afterSaveAction">An optional synchronous delegate that allows executing custom logic after saving changes. (Action)</param>
  /// <returns>The number of entities saved.</returns>
  int SaveChanges(Action? beforeSaveAction = null, Action? afterSaveAction = null);
}
