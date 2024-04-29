namespace Buzruk.GenericRepository;

/// <summary>
/// This class represents the results of a paginated query, containing retrieved entities and pagination information.
/// </summary>
/// <typeparam name="T">The type of entity contained within the results.</typeparam>
public class PagedResults<T>
{
  /// <summary>
  /// The collection of items retrieved for the current page.
  /// </summary>
  public IEnumerable<T> Items { get; set; } = [];

  /// <summary>
  /// The total number of entities matching the query criteria.
  /// </summary>
  public int TotalItemsCount { get; set; }

  /// <summary>
  /// The total number of pages available based on the page size.
  /// </summary>
  public int TotalPages { get; set; }

  /// <summary>
  /// The number of items retrieved per page.
  /// </summary>
  public int PageSize { get; set; }

  /// <summary>
  /// The current page number of the retrieved results.
  /// </summary>
  public int PageNumber { get; set; }

  /// <summary>
  /// Indicates whether a previous page exists based on the current page number.
  /// </summary>
  public bool HasPreviousPage { get; set; }

  /// <summary>
  /// Indicates whether a next page exists based on the current page number and total pages.
  /// </summary>
  public bool HasNextPage { get; set; }

  /// <summary>
  /// A flag indicating whether the current page is the first page.
  /// </summary>
  public bool IsFirstPage { get; set; }

  /// <summary>
  /// A flag indicating whether the current page is the last page.
  /// </summary>
  public bool IsLastPage { get; set; }

  /// <summary>
  /// The zero-based index of the first item on the current page.
  /// </summary>
  public int FirstItemOnPage { get; set; }

  /// <summary>
  /// The zero-based index of the last item on the current page, 
  /// considering the total number of items and page size.
  /// </summary>
  public int LastItemOnPage { get; set; }
}

