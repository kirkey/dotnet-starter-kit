using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;
public class GetBrandRequest(Guid id) : IRequest<BrandResponse>
{
    public Guid Id { get; set; } = id;
}
