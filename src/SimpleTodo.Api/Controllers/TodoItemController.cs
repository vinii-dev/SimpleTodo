using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleTodo.Application.Services;
using SimpleTodo.Domain.Common;
using SimpleTodo.Domain.Contracts.Pagination;
using SimpleTodo.Domain.DTOs.TodoItems;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace SimpleTodo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoItemController(ITodoItemService todoItemService) : ControllerBase
{
    public Guid UserId => Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieves a paginated list of TodoItems.")]
    [ProducesResponseType(typeof(PagedList<TodoItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request, CancellationToken cancellationToken)
    {
        var todoItems = await todoItemService.GetPagedAsync(UserId, request, cancellationToken);

        return Ok(todoItems);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Retrieves a TodoItem by its ID.")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemService.GetByIdAsync(UserId, id, cancellationToken);
        if (todoItem is null)
            return NotFound();

        return Ok(todoItem);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new TodoItem.")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] TodoItemCreateDto todoItemCreateDto, CancellationToken cancellationToken)
    {
        var todoItemId = await todoItemService.CreateAsync(UserId, todoItemCreateDto, cancellationToken);

        return Created($"/api/{todoItemId}", todoItemId);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Updates an existing TodoItem.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] TodoItemUpdateDto todoItemUpdateDto, CancellationToken cancellationToken)
    {
        await todoItemService.UpdateAsync(UserId, id, todoItemUpdateDto, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Partially updates an existing TodoItem.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] TodoItemPatch todoItemPatchDto, CancellationToken cancellationToken)
    {
        await todoItemService.PatchAsync(UserId, id, todoItemPatchDto, cancellationToken);

        return NoContent();
    }
}
