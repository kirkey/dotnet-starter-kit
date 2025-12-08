using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Create.v1;

public sealed record CreateCollateralReleaseCommand(
    DefaultIdType CollateralId,
    DefaultIdType LoanId,
    string ReleaseReference,
    DefaultIdType RequestedById,
    string? ReleaseMethod = null) : IRequest<CreateCollateralReleaseResponse>;
