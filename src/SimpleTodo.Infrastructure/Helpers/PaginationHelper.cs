using Microsoft.EntityFrameworkCore;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;

namespace SimpleTodo.Infrastructure.Helpers;

/// <summary>
/// Provides helper methods for creating paginated lists.
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Creates a paginated list asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="items">The queryable collection of items to paginate.</param>
    /// <param name="page">The current page number. Must be greater than zero.</param>
    /// <param name="pageSize">The number of items per page. Must be greater than zero.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an instance of <see cref="PagedList{T}"/>, which encapsulates the items for 
    /// the current page and metadata including the current page, page size, total count, and total pages.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="page"/> or <paramref name="pageSize"/> is less than or equal to zero.
    /// </exception>
    public async static Task<PagedList<T>> CreateAsync<T>(IQueryable<T> items, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(page);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        var totalCount = await items.CountAsync(cancellationToken);

        var pagedItems = await items.Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync(cancellationToken);

        return new PagedList<T>(pagedItems, page, pageSize, totalCount);
    }

    /// <summary>
    /// Creates a paginated list asynchronously using a <see cref="PaginationParameters"/> object.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="items">The queryable collection of items to paginate.</param>
    /// <param name="parameters">The pagination parameters (page number and size).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result is a instance of <see cref="PagedList{T}"/>.
    /// </returns>
    /// <remarks>
    /// This method delegates to <see cref="CreateAsync{T}(IQueryable{T}, int, int, CancellationToken)"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="parameters.Page"/> or <paramref name="parameters.PageSize"/> is invalid.
    /// </exception>
    public async static Task<PagedList<T>> CreateAsync<T>(IQueryable<T> items, PaginationParameters parameters, CancellationToken cancellationToken = default)
        => await CreateAsync(items, parameters.Page, parameters.PageSize, cancellationToken);
}
