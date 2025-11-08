namespace Accounting.Application.DepreciationMethods.Activate.v1;

public sealed record ActivateDepreciationMethodCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

