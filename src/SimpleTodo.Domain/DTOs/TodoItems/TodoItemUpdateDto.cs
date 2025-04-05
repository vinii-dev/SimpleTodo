using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Domain.DTOs.TodoItems;

[SwaggerSchema("DTO for updating a new Todo item")]
public record TodoItemUpdateDto(
    [SwaggerSchema("Title of the Todo item"), Required]
    string Title,

    [SwaggerSchema("Description of the Todo item"), Required]
    string Desription);
