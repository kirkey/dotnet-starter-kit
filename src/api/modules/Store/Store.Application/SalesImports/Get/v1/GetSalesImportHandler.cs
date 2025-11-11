using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Get.v1;

/// <summary>
/// Handler for retrieving detailed sales import information.
/// </summary>
public class GetSalesImportHandler(IReadRepository<SalesImport> repository)
    : IRequestHandler<GetSalesImportRequest, SalesImportDetailResponse>
{
    public async Task<SalesImportDetailResponse> Handle(GetSalesImportRequest request, CancellationToken cancellationToken)
    {
        var salesImport = await repository.FirstOrDefaultAsync(
            new SalesImportByIdWithItemsSpec(request.Id), cancellationToken);

        if (salesImport == null)
        {
            throw new NotFoundException($"Sales import with ID {request.Id} not found");
        }

        var response = salesImport.Adapt<SalesImportDetailResponse>();
        return response;
    }
}

