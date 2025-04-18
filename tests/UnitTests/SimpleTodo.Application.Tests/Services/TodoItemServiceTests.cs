﻿using NSubstitute;
using SimpleTodo.Application.Services;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.DTOs.TodoItems;
using SimpleTodo.Domain.Entities;
using SimpleTodo.Domain.Errors;
using SimpleTodo.Domain.Interfaces.Repositories;
using TestCommon.Asserts;
using TestCommon.Consts;
using TestCommon.Factory;

namespace SimpleTodo.Application.Tests.Services;

public class TodoItemServiceTests
{
    private readonly ITodoItemRepository _todoItemRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly TodoItemService _todoItemService;
    private readonly Guid UserId;

    public TodoItemServiceTests()
    {
        _todoItemRepositoryMock = Substitute.For<ITodoItemRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _todoItemService = new TodoItemService(_todoItemRepositoryMock, _userRepositoryMock);
        UserId = Guid.NewGuid();
    }

    [Fact]
    public async Task GetPagedAsync_UserExists_ReturnsPagedTodoItems()
    {
        // Arrange
        var pagination = new PaginationParameters(1, 10);
        var totalCount = 1;
        var todoItems = new List<TodoItem>
        {
            TodoItemFactory.CreateTodoItem(userId: UserId),
        };
        var pagedItems = new PagedList<TodoItem>(todoItems, pagination.Page, pagination.PageSize, totalCount);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetPagedByUserIdAsync(UserId, pagination)
            .Returns(pagedItems);

        // Act
        var result = await _todoItemService.GetPagedAsync(UserId, pagination);

        // Assert
        Assert.Equal(todoItems.Count, result.Items.Count);
        Assert.Equal(todoItems[0].Title, result.Items.First().Title);
    }

    [Fact]
    public async Task GetPagedAsync_UserNotExists_ReturnsEmptyPagedList()
    {
        // Arrange
        var pagination = new PaginationParameters(1, 10);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(false);

        // Act
        var result = await _todoItemService.GetPagedAsync(UserId, pagination);

        // Assert
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetByIdAsync_UserExistsAndItemExists_ReturnsTodoItem()
    {
        // Arrange
        var todoItem = TodoItemFactory.CreateTodoItem(userId: UserId);
        var todoItemId = todoItem.Id;

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.GetByIdAsync(UserId, todoItemId);

        // Assert
        Assert.False(result.IsError);

        var createdTodoItem = result.Value;
        Assert.NotNull(createdTodoItem);
        Assert.Equal(todoItem.Title, createdTodoItem!.Title);
        Assert.Equal(todoItem.Description, createdTodoItem!.Description);
        Assert.Equal(todoItem.IsCompleted, createdTodoItem!.IsCompleted);
        Assert.Equal(todoItem.Id, createdTodoItem!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_UserNotExists_ReturnsUserNotFoundError()
    {
        // Arrange
        var todoItemId = Guid.NewGuid();
        _userRepositoryMock.ExistsAsync(UserId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _todoItemService.GetByIdAsync(UserId, todoItemId);

        // Assert
        ErrorOrAssert.IsError(result, UserErrors.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_ItemNotExists_ReturnsNull()
    {
        // Arrange
        var todoItemId = Guid.NewGuid();

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns((TodoItem?)null);

        // Act
        var result = await _todoItemService.GetByIdAsync(UserId, todoItemId);

        // Assert
        ErrorOrAssert.IsError(result, TodoItemErrors.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_ItemBelongsToDifferentUser_ReturnsNotFoundTodoItemError()
    {
        // Arrange
        var otherUserId = Guid.NewGuid();
        var todoItem = TodoItemFactory.CreateTodoItem(userId: otherUserId);
        var todoItemId = todoItem.Id;

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.GetByIdAsync(UserId, todoItemId);

        // Assert
        ErrorOrAssert.IsError(result, TodoItemErrors.NotFound);
    }

    [Fact]
    public async Task CreateAsync_UserExists_CreatesAndReturnsId()
    {
        // Arrange
        var createDto = new TodoItemCreateDto(TodoItemConsts.Title, TodoItemConsts.Description);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);

        // Act
        var result = await _todoItemService.CreateAsync(UserId, createDto);

        // Assert
        Assert.False(result.IsError);
        await _todoItemRepositoryMock.Received(1).CreateAsync(
            Arg.Is<TodoItem>(t => t.Title == createDto.Title && t.Description == createDto.Description));
    }

    [Fact]
    public async Task CreateAsync_UserNotExists_ReturnsNotFoundUserError()
    {
        // Arrange
        var createDto = new TodoItemCreateDto(TodoItemConsts.Title, TodoItemConsts.Description);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(false);

        // Act
        var result = await _todoItemService.CreateAsync(UserId, createDto);

        // Assert
        ErrorOrAssert.IsError(result, UserErrors.NotFound);
    }

    [Fact]
    public async Task RemoveAsync_UserAndItemExist_RemovesItem()
    {
        // Arrange
        var todoItem = TodoItemFactory.CreateTodoItem(userId: UserId);
        var todoItemId = todoItem.Id;

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.RemoveAsync(UserId, todoItemId);

        // Assert
        Assert.False(result.IsError);
        await _todoItemRepositoryMock.Received(1).RemoveAsync(todoItem);
    }

    [Fact]
    public async Task RemoveAsync_UserNotExists_ReturnsNotFoundUserError()
    {
        // Arrange
        var todoItemId = Guid.NewGuid();
        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(false);

        // Act
        var result = await _todoItemService.RemoveAsync(UserId, todoItemId);

        // Assert
        ErrorOrAssert.IsError(result, UserErrors.NotFound);
    }

    [Fact]
    public async Task RemoveAsync_ItemNotExists_ReturnNotFoundTodoItemError()
    {
        // Arrange
        var todoItemId = Guid.NewGuid();

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);

        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns((TodoItem?)null);

        // Act
        var result = await _todoItemService.RemoveAsync(UserId, todoItemId);

        // Assert
        ErrorOrAssert.IsError(result, TodoItemErrors.NotFound);
    }

    [Fact]
    public async Task RemoveAsync_ItemBelongsToDifferentUser_ReturnNotFoundTodoItemError()
    {
        // Arrange
        var otherUserId = Guid.NewGuid();
        var todoItem = TodoItemFactory.CreateTodoItem(userId: otherUserId);
        var todoItemId = todoItem.Id;

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.RemoveAsync(UserId, todoItemId);

        // Assert
        ErrorOrAssert.IsError(result, TodoItemErrors.NotFound);
    }

    [Fact]
    public async Task UpdateAsync_ValidData_UpdatesItem()
    {
        // Arrange
        var todoItem = TodoItemFactory.CreateTodoItem(userId: UserId);
        var todoItemId = todoItem.Id;
        var newTitle = "New Title";
        var newDescription = "New Description";
        var updateDto = new TodoItemUpdateDto(newTitle, newDescription);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);

        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.UpdateAsync(UserId, todoItemId, updateDto);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(updateDto.Title, todoItem.Title);
        Assert.Equal(updateDto.Description, todoItem.Description);
        await _todoItemRepositoryMock.Received(1).UpdateAsync(todoItem);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task PatchAsync_UpdateIsCompleted_TogglesCompletedStatus(bool originalIsCompleted, bool patchIsCompleted)
    {
        // Arrange
        var todoItem = TodoItemFactory.CreateTodoItem(userId: UserId, isCompleted: originalIsCompleted);
        var todoItemId = todoItem.Id;
        var patchDto = new TodoItemPatch(IsCompleted: patchIsCompleted);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);

        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.PatchAsync(UserId, todoItemId, patchDto);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(todoItem.IsCompleted, patchIsCompleted);
        await _todoItemRepositoryMock.Received(1).UpdateAsync(todoItem);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task PatchAsync_NoChanges_DoesNotChangeIsCompletedStatus(bool isCompleted)
    {
        // Arrange
        var todoItem = TodoItemFactory.CreateTodoItem(userId: UserId, isCompleted: isCompleted);
        var todoItemId = todoItem.Id;
        var patchDto = new TodoItemPatch(isCompleted);

        _userRepositoryMock.ExistsAsync(UserId)
            .Returns(true);
        _todoItemRepositoryMock.GetByIdAsync(todoItemId)
            .Returns(todoItem);

        // Act
        var result = await _todoItemService.PatchAsync(UserId, todoItemId, patchDto);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(todoItem.IsCompleted, isCompleted);
    }
}
