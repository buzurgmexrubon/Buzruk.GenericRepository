namespace Buzruk.GenericRepository;

public interface IUnitOfWorkAsync
{
  Task<IGenericRepositoryAsync<T>> GetRepositoryAsync<T>(CancellationToken cancellationToken = default) where T : class;

  Task<int> SaveChangesAsync(Func<Task>? beforeSaveAction = null,
                                    Func<Task>? afterSaveAction = null,
                                    CancellationToken cancellationToken = default);
}
