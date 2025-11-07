namespace Accounting.Application.RetainedEarnings.UpdateNetIncome.v1;

public sealed record UpdateNetIncomeCommand(DefaultIdType Id, decimal NetIncome) : IRequest<DefaultIdType>;
