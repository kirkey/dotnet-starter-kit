using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;
public class GetProductRequest(DefaultIdType id) : IRequest<ProductResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
