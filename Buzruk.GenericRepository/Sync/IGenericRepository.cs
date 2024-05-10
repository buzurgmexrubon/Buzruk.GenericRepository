namespace Buzruk.GenericRepository;

public interface IGenericRepository<T> where T : class
{
  #region CRUD (Get, GetPaged, Add, AddRange, Update, UpdateRange, Remove, RemoveRange)

  T Get(
      Expression<Func<T, bool>> predicate,
      bool tracking = false);


  IEnumerable<T> GetAll(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      bool tracking = false);

  PagedResults<T> GetPaged(
      Expression<Func<T, bool>>[]? predicates = null,
      Expression<Func<T, object>>? orderBy = null,
      Expression<Func<T, object>>? thenBy = null,
      int pageNumber = 1,
      int pageSize = int.MaxValue,
    bool tracking = false);

  void Add(T entity);

  void AddRange(IEnumerable<T> entities);

  void Update(T entity);

  void UpdateRange(IEnumerable<T> entities);

  void Remove(T entity);

  void Remove(Expression<Func<T, bool>> keyPredicate);

  void RemoveRange(IEnumerable<T> entities);

  #endregion

  #region Other Methods (Exists, Count, LongCount, CountBy)

  bool Exists(Expression<Func<T, bool>> predicate);

  int Count(Expression<Func<T, bool>>? predicate = null);

  long LongCount(Expression<Func<T, bool>>? predicate = null);

  int CountBy<TProperty>(Expression<Func<T, TProperty>> groupExpression,
                                           Expression<Func<T, bool>>? predicate = null);

  #endregion
}
