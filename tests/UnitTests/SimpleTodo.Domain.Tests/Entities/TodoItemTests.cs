using SimpleTodo.Domain.Entities;
using TestCommon.Consts;
using TestCommon.Factory;

namespace SimpleTodo.Domain.Tests.Entities;

public class TodoItemTests
{
    private readonly Guid _userId;
    public TodoItemTests()
    {
        _userId = Guid.NewGuid();
    }

    [Fact]
    public void Constructor_ValidInput_CreatesTodoItem()
    {
        // Act
        var item = new TodoItem(TodoItemConsts.Title, TodoItemConsts.Description, _userId);

        // Assert
        Assert.Equal(TodoItemConsts.Title, item.Title);
        Assert.Equal(TodoItemConsts.Description, item.Description);
        Assert.Equal(_userId, item.UserId);
        Assert.False(item.IsCompleted);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_TitleNullOrWhitespace_ThrowsArgumentException(string? title)
    {
        // Arrange
        var instantiateTodoItem = () => new TodoItem(title!, TodoItemConsts.Description, _userId);

        // Act
        Assert.Throws<ArgumentException>(instantiateTodoItem);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_DescriptionNullOrWhitespace_ThrowsArgumentException(string? description)
    {
        // Arrange
        var instantiateTodoItem = () => new TodoItem(TodoItemConsts.Title, description!, _userId);

        // Act
        Assert.Throws<ArgumentException>(instantiateTodoItem);
    }

    [Fact]
    public void Constructor_UserIdEmpty_ThrowsArgumentException()
    {
        // Arrange
        var instantiateTodoItem = () =>
            new TodoItem(TodoItemConsts.Title, TodoItemConsts.Description, Guid.Empty);

        // Act
        Assert.Throws<ArgumentException>(instantiateTodoItem);
    }

    [Fact]
    public void Update_ValidInput_UpdatesProperties()
    {
        // Arrange
        var item = TodoItemFactory.CreateTodoItem();
        var newTitle = "New Title";
        var newDescription = "New Description";

        // Act
        item.Update(newTitle, newDescription);

        // Assert
        Assert.Equal(newTitle, item.Title);
        Assert.Equal(newDescription, item.Description);
    }

    [Fact]
    public void ToggleCompleted_WhenNotCompleted_SetsIsCompletedToTrue()
    {
        // Arrange
        var item = TodoItemFactory.CreateTodoItem();

        // Act
        item.ToggleCompleted();

        // Assert
        Assert.True(item.IsCompleted);
    }

    [Fact]
    public void ToggleCompleted_WhenCompleted_SetsIsCompletedToFalse()
    {
        // Arrange
        var item = TodoItemFactory.CreateTodoItem(isCompleted: true);

        // Act
        item.ToggleCompleted();

        // Assert
        Assert.False(item.IsCompleted);
    }

}

