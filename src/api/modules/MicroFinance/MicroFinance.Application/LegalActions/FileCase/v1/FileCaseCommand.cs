using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.FileCase.v1;

public sealed record FileCaseCommand(
    Guid Id,
    DateOnly FiledDate,
    string CaseReference,
    string CourtName,
    decimal CourtFees) : IRequest<FileCaseResponse>;
