using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Create.v1;

public sealed record CreateCollateralReleaseCommand(
    Guid CollateralId,
    Guid LoanId,
    string ReleaseReference,
    Guid RequestedById,
    string? ReleaseMethod = null) : IRequest<CreateCollateralReleaseResponse>;
