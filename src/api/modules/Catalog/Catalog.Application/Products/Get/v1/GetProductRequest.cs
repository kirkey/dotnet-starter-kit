using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;
public class GetProductRequest(Guid id) : IRequest<ProductResponse>
{
    public Guid Id { get; set; } = id;
}
