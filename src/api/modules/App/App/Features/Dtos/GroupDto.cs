﻿namespace FSH.Starter.WebApi.App.Features.Dtos;

public record GroupDto(
    DefaultIdType Id,
    string Application,
    string Parent,
    string? Tag,
    int Number,
    string Code,
    string Name,
    decimal Amount,
    DefaultIdType? EmployeeId,
    string? EmployeeName,
    string? Description,
    string? Notes);
