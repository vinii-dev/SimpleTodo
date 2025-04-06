using SimpleTodo.Domain.Common;

namespace SimpleTodo.Domain.Tests.Common;

public class PagedResourceTests
{
    private readonly List<int> _items;
    private const int _currentPage = 1;
    private const int _totalCount = 10;
    private const int _pageSize = 5;

    public PagedResourceTests()
    {
        _items = Enumerable.Range(1, _totalCount).ToList();
    }

    [Fact]
    public void Constructor_ValidParams_InitializeCorrectly()
    {
        // Arrange
        var items = _items.Take(_pageSize).ToList();

        // Act
        var pagedResource = new PagedList<int>(items, _currentPage, _pageSize, _totalCount);

        // Assert
        Assert.Equal(items, pagedResource.Items);
        Assert.Equal(_currentPage, pagedResource.CurrentPage);
        Assert.Equal(_pageSize, pagedResource.PageSize);
        Assert.Equal(_totalCount, pagedResource.TotalCount);
    }

    [Fact]
    public void Constructor_ItemsCountGreaterThanPageSize_ThrowsArgumentException()
    {
        // Arrange
        var pageSize = _pageSize - 10;
        Action instantiatePagedList = () => new PagedList<int>(_items, _currentPage, pageSize, _totalCount);

        // Act & Assert
        Assert.Throws<ArgumentException>(instantiatePagedList);
    }

    [Theory]
    [InlineData(1, 10, 100, 10, false, true)]
    [InlineData(2, 10, 100, 10, true, true)]
    [InlineData(10, 10, 100, 10, true, false)]
    [InlineData(1, 10, 5, 1, false, false)]
    [InlineData(1, 10, 0, 0, false, false)]
    public void ComputedProperties_ReturnCorrectValues(
        int currentPage,
        int pageSize,
        int totalCount,
        int expectedTotalPages,
        bool expectedHasPrevious,
        bool expectedHasNext)
    {
        // Arrange
        var items = new List<int>(new int[Math.Min(pageSize, totalCount)]);
        var pagedList = new PagedList<int>(items, currentPage, pageSize, totalCount);

        // Assert
        Assert.Equal(expectedTotalPages, pagedList.TotalPages);
        Assert.Equal(expectedHasPrevious, pagedList.HasPreviousPage);
        Assert.Equal(expectedHasNext, pagedList.HasNextPage);
    }

}

