using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Domain.DTOs.TodoItems;

[SwaggerSchema("DTO for creating a new Todo item")]
public record TodoItemCreateDto(
    [SwaggerSchema("Title of the Todo item"), Required]
    string Title,

    [SwaggerSchema("Description of the Todo item"), Required]
    string Description);
