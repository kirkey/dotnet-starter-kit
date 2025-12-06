using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Close.v1;

/// <summary>
/// Command for closing a collection case.
/// </summary>
public sealed record CloseCollectionCaseCommand(
    Guid Id,
    string? Notes = null) : IRequest<CloseCollectionCaseResponse>;
