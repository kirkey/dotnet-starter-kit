﻿namespace FSH.Starter.WebApi.Todo.Features.Get.v1;
public record GetTodoResponse(DefaultIdType? Id, string? Name, string? Description, string? Notes);
