namespace Buzruk.GenericRepository.Async;

public interface IUnitOfWorkAsync
{
  Task<IGenericRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class;

  Task<int> SaveChangesAsync(Func<Task>? beforeSaveAction = null,
                                    Func<Task>? afterSaveAction = null,
                                    CancellationToken cancellationToken = default);
}
