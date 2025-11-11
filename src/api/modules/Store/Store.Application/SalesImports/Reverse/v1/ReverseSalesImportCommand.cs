namespace FSH.Starter.WebApi.Store.Application.SalesImports.Reverse.v1;

/// <summary>
/// Command to reverse a sales import, creating offsetting inventory transactions.
/// </summary>
public class ReverseSalesImportCommand : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string Reason { get; set; } = default!;
}

