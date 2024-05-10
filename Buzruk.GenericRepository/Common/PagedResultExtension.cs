namespace Buzruk.GenericRepository;

/// <summary>
/// Provides an extension method for the `PagedResults<T>` class to extract a specific page of data.
/// </summary>
public static class PagedResultsExtension
{
  /// <summary>
  /// Retrieves a single page of data from the `PagedResults<T>` object.
  /// </summary>
  /// <typeparam name="T">The type of entity contained within the results.</typeparam>
  /// <param name="pagedResults">The `PagedResults<T>` object representing the paginated query results.</param>
  /// <returns>An `IEnumerable<T>` containing the items for the specified page.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown if the requested page number is outside the valid range (1 to `PageCount`).</exception>
  public static IEnumerable<T> GetPage<T>(this PagedResults<T> pagedResults)
  => pagedResults.Items.Skip(pagedResults.PageSize * (pagedResults.PageNumber - 1))
                     .Take(pagedResults.PageSize)
                     .ToList();

  public static async Task<PagedResults<T>> ToPagedResultAsync<T>(IQueryable<T> source, 
                                                             int pageNumber,
                                                             int pageSize)
  {
    int totalItemsCount = await source.CountAsync();
    int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

    var results = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    return new PagedResults<T>
    {
      Items = results,
      TotalItemsCount = totalItemsCount,
      TotalPages = totalPages,
      PageSize = pageSize,
      PageNumber = pageNumber,
      HasPreviousPage = pageNumber > 1,
      HasNextPage = pageNumber < totalPages,
      IsFirstPage = pageNumber == 1,
      IsLastPage = pageNumber == totalPages,
      FirstItemOnPage = (pageNumber - 1) * pageSize + 1,
      LastItemOnPage = Math.Min(pageNumber * pageSize, totalItemsCount)
    };
  }

  public static PagedResults<T> ToPagedResult<T>(IEnumerable<T> source, 
                                                             int pageNumber,
                                                             int pageSize)
  {
    int totalItemsCount = source.Count();
    int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

    var results = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    return new PagedResults<T>
    {
      Items = results,
      TotalItemsCount = totalItemsCount,
      TotalPages = totalPages,
      PageSize = pageSize,
      PageNumber = pageNumber,
      HasPreviousPage = pageNumber > 1,
      HasNextPage = pageNumber < totalPages,
      IsFirstPage = pageNumber == 1,
      IsLastPage = pageNumber == totalPages,
      FirstItemOnPage = (pageNumber - 1) * pageSize + 1,
      LastItemOnPage = Math.Min(pageNumber * pageSize, totalItemsCount)
    };
  }

  public static PagedResults<T> ToPagedResult<T>(IQueryable<T> source, 
                                                             int pageNumber,
                                                             int pageSize)
  {
    int totalItemsCount = source.Count();
    int totalPages = (int)Math.Ceiling((double)totalItemsCount / pageSize);

    var results = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    return new PagedResults<T>
    {
      Items = results,
      TotalItemsCount = totalItemsCount,
      TotalPages = totalPages,
      PageSize = pageSize,
      PageNumber = pageNumber,
      HasPreviousPage = pageNumber > 1,
      HasNextPage = pageNumber < totalPages,
      IsFirstPage = pageNumber == 1,
      IsLastPage = pageNumber == totalPages,
      FirstItemOnPage = (pageNumber - 1) * pageSize + 1,
      LastItemOnPage = Math.Min(pageNumber * pageSize, totalItemsCount)
    };
  }


  /// <summary>
  /// Generates a partial view containing HTML code for page navigation based on the provided `PagedResults<T>` model.
  /// This method leverages Razor syntax (`@` symbol) for building the HTML structure.
  /// </summary>
  /// <param name="model">The `PagedResults<T>` instance containing paging information and data.</param>
  /// <param name="controllerName">The name of the controller that handles pagination requests.</param>
  /// <param name="actionName">The name of the action method that handles pagination requests.</param>
  /// <param name="areaName">The optional area name for routing (defaults to empty string).</param>
  /// <param name="routeParameterName">The name of the route parameter used for the current page number (defaults to "pageNumber").</param>
  /// <returns>A string containing the generated HTML partial view for pagination.</returns>
  public static string GetPartialView<T>(this PagedResults<T> model, string controllerName, string actionName, string areaName = "", string routeParameterName = "pageNumber")
  {
    // TODO: Create PaginationPartialConfig class that includes string controllerName, string actionName, string areaName, string routeParameterName
    // and extend it (like adding dark-mode, light-mode...)
    StringBuilder view = new();

    if (model.TotalPages > 1)
    {
      view.AppendLine("<div class=\"d-flex justify-content-center\">");
      view.AppendLine("   <nav aria-label=\"Page Navigation Example\">");
      view.AppendLine("       <ul class=\"pagination\">");

      if (model.TotalPages < 6)
      {
        for (int i = 1; i <= model.TotalPages; i++)
        {
          string active = string.Empty;
          if (model.PageNumber == i)
          {
            active = " active";
          }

          view.AppendLine($"<li class=\"page-item{active}\">");
          view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={i}\" >{i}</a>");
          view.AppendLine($"</li>");
        }
      }
      else if (model.PageNumber < 3)
      {
        string previousButtonState = string.Empty;
        if (model.PageNumber == 1)
        {
          previousButtonState = " disabled";
        }
        view.AppendLine($"<li class=\"page-item{previousButtonState}\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber - 1}\">");
        view.AppendLine("         <span area-hidden=\"true\">&laquo;</span>");
        view.AppendLine("     </a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={1}\">1</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={2}\">2</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={3}\">3</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link disabled\" >...</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.TotalPages}\">{model.TotalPages}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item{previousButtonState}\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber + 1}\">");
        view.AppendLine("         <span area-hidden=\"true\">&raquo;</span>");
        view.AppendLine("     </a>");
        view.AppendLine("</li>");
      }
      else if (model.TotalPages - 2 >= model.PageNumber)
      {

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber - 1}\">");
        view.AppendLine("         <span area-hidden=\"true\">&laquo;</span>");
        view.AppendLine("     </a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={1}\">1</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link disabled\" >...</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber - 1}\">{model.PageNumber - 1}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber}\">{model.PageNumber}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber + 1}\">{model.PageNumber + 1}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link disabled\" >...</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.TotalPages}\">{model.TotalPages}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber + 1}\">");
        view.AppendLine("         <span area-hidden=\"true\">&raquo;</span>");
        view.AppendLine("     </a>");
        view.AppendLine("</li>");
      }
      else
      {
        string nextButtonState = string.Empty;
        if (model.PageNumber == model.TotalPages)
        {
          nextButtonState = " disabled";
        }

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber - 1}\">");
        view.AppendLine("         <span area-hidden=\"true\">&laquo;</span>");
        view.AppendLine("     </a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={1}\">1</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link disabled\" >...</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.TotalPages - 2}\">{model.TotalPages - 2}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.TotalPages - 1}\">{model.TotalPages - 1}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.TotalPages}\">{model.TotalPages}</a>");
        view.AppendLine("</li>");

        view.AppendLine($"<li class=\"page-item{nextButtonState}\">");
        view.AppendLine($"    <a class=\"page-link\" href=\"{areaName}/{controllerName}/{actionName}?{routeParameterName}={model.PageNumber + 1}\">");
        view.AppendLine("         <span area-hidden=\"true\">&raquo;</span>");
        view.AppendLine("     </a>");
        view.AppendLine("</li>");
      }

      view.AppendLine("       </ul>");
      view.AppendLine("   </nav>");
      view.AppendLine("</div>");

      view.AppendLine("<script>");
      // making active page with active class
      // TODO: Write this login in C#
      view.AppendLine($"var pageNumber = {model.PageNumber}");
      view.AppendLine("var items = document.getElementsByClassName(\"page-item\")");
      view.AppendLine("for (var i = 0; i < items.length; i++) {");
      view.AppendLine("    if (items[i].children[0].innerText == pageNumber) {");
      view.AppendLine("        items[i].classList.add(\"active\")");
      view.AppendLine("    }");
      view.AppendLine("}");

      // Get domain name
      view.AppendLine("   var aTags = document.getElementsByTagName(\"a\")");
      view.AppendLine("   var rootUrl = window.location.origin");
      view.AppendLine("   for (var i = 0; i < aTags.length; i++) {");
      view.AppendLine("       var href = aTags[i].getAttribute(\"href\")");
      view.AppendLine("       if (href != null && href.startsWith(\"/\")) {");
      view.AppendLine("           aTags[i].setAttribute(\"href\", rootUrl + href.substring(1))");
      view.AppendLine("       }");
      view.AppendLine("   }");
      view.AppendLine("</script>");

      return view.ToString();
    }
    return string.Empty;
  }
}
