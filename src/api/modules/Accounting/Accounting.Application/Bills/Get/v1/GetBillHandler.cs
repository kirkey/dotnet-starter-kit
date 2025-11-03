namespace Accounting.Application.Bills.Get.v1;

/// <summary>
/// Handler for getting a bill by ID.
/// </summary>
public sealed class GetBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository)
    : IRequestHandler<GetBillRequest, BillResponse>
{
    public async Task<BillResponse> Handle(GetBillRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetBillByIdSpec(request.BillId);
        var bill = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        return bill.Adapt<BillResponse>();
    }
}

