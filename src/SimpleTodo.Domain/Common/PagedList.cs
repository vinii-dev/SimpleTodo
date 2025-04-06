namespace SimpleTodo.Domain.Common;

/// <summary>
/// Represents a paginated resource with metadata about the pagination state.
/// </summary>
/// <typeparam name="T">The type of items in the paginated resource.</typeparam>
public class PagedList<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items in the current page.</param>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalCount">The total number of items.</param>
    public PagedList(List<T> items, int currentPage, int pageSize, int totalCount)
    {
        if (items.Count > pageSize)
            throw new ArgumentException("Items count cannot be greater than page size.", nameof(items));

        Items = items;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Gets an empty instance of <see cref="PagedList{T}"/>.
    /// </summary>
    public static PagedList<T> Empty => new PagedList<T>([], 0, 0, 0);

    /// <summary>
    /// Gets or sets the items in the current page.
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool IsPreviousPageExists => CurrentPage > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool IsNextPageExists => CurrentPage < TotalPages;
}
