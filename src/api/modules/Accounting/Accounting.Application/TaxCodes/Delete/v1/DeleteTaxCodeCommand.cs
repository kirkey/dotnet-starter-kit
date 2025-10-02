namespace Accounting.Application.TaxCodes.Delete.v1;

public sealed record DeleteTaxCodeCommand(DefaultIdType Id) : IRequest;
