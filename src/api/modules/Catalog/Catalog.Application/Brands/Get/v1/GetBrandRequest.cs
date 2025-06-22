using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;
public class GetBrandRequest(DefaultIdType id) : IRequest<BrandResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
