namespace Accounting.Application.DepreciationMethods.Deactivate.v1;

public sealed record DeactivateDepreciationMethodCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

