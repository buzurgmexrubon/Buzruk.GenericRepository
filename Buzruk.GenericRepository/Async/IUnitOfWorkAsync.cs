namespace Buzruk.GenericRepository.Async;

public interface IUnitOfWorkAsync
{
  Task<IGenericRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class;

  /// <summary>
  /// Asynchronously saves all changes made to entities tracked by the context to the underlying database. 
  /// This method offers flexibility for executing custom logic before and after saving changes.
  /// </summary>
  /// <param name="beforeSaveAction">An optional asynchronous delegate that allows executing custom logic before saving changes. (Func&lt;Task&gt;)</param>
  /// <param name="afterSaveAction">An optional asynchronous delegate that allows executing custom logic after saving changes. (Func&lt;Task&gt;)</param>
  /// <returns>A task that returns the number of entities saved.</returns>
  Task<int> SaveChangesAsync(Func<Task>? beforeSaveAction = null,
                                    Func<Task>? afterSaveAction = null,
                                    CancellationToken cancellationToken = default);
}
