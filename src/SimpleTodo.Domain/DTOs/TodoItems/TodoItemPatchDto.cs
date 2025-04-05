﻿using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SimpleTodo.Domain.DTOs.TodoItems;

[SwaggerSchema("DTO for patching a Todo item")]
public record TodoItemPatch(
    [SwaggerSchema("Indicates if the Todo item is completed", Nullable = true)]
    bool? IsCompleted);
