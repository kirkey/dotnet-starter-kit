﻿namespace FSH.Starter.WebApi.Todo.Features.GetList.v1;

/// <summary>
/// DTO for displaying a todo item in list views.
/// Contains all displayable information for a todo item.
/// </summary>
public record TodoDto(
    /// <summary>The unique identifier of the todo item.</summary>
    DefaultIdType? Id,
    /// <summary>The name of the todo item.</summary>
    string Name,
    /// <summary>The description of the todo item.</summary>
    string Description,
    /// <summary>Additional notes for the todo item.</summary>
    string Notes);

