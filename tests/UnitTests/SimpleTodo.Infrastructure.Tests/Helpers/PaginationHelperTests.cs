

using SimpleTodo.Infrastructure.Helpers;
using MockQueryable;
using SimpleTodo.Domain.Contracts.Pagination;

namespace SimpleTodo.Infrastructure.Tests.Helpers;

internal class IntEntityTest(int number)
{
    public int Number { get; } = number;
}

public class PaginationHelperTests
{
    private const int _totalCount = 30;
    private const int _page = 1;
    private const int _pageSize = 10;
    private readonly IQueryable<IntEntityTest> _queryableItems;

    public PaginationHelperTests()
    {
        var items = Enumerable.Range(1, _totalCount).Select(n => new IntEntityTest(n));
        _queryableItems = items.BuildMock();
    }

    [Fact]
    public async Task CreateAsync_WithPaginationParameters_DelegatesCorrectly()
    {
        // Arrange
        var page = 2;
        var pageSize = 20;
        var parameters = new PaginationParameters(Page: page, PageSize: pageSize);

        // Act
        var result = await PaginationHelper.CreateAsync(_queryableItems, parameters);

        // Assert
        Assert.Equal(page, result.CurrentPage);
        Assert.Equal(pageSize, result.PageSize);
    }

    [Fact]
    public async Task CreateAsync_ValidParameters_ReturnsExpectedPagedList()
    {
        // Arrange
        var page = 2;
        var startPosition = ((page - 1) * _pageSize) + 1;
        var expectedItems = Enumerable.Range(startPosition, _pageSize).ToList();

        // Act
        var pagedList = await PaginationHelper.CreateAsync(_queryableItems, page, _pageSize);

        // Assert
        Assert.NotNull(pagedList);
        Assert.Equal(page, pagedList.CurrentPage);
        Assert.Equal(_pageSize, pagedList.PageSize);
        Assert.Equal(_totalCount, pagedList.TotalCount);

        Assert.All(pagedList.Items, item =>
        {
            var idx = pagedList.Items.IndexOf(item);
            Assert.Equal(expectedItems[idx], item.Number);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task CreateAsync_InvalidPage_ThrowsArgumentOutOfRangeException(int page)
    {
        // Arrange
        var pageSize = 10;
        Func<Task> createAsync = () => PaginationHelper.CreateAsync(_queryableItems, page, pageSize);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(createAsync);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task CreateAsync_InvalidPageSize_ThrowsArgumentOutOfRangeException(int pageSize)
    {
        // Arrange
        var page = 1;
        Func<Task> createAsync = () => PaginationHelper.CreateAsync(_queryableItems, page, pageSize);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(createAsync);
    }
}
