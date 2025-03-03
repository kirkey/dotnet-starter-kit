namespace FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;
public sealed record BrandResponse(DefaultIdType? Id, string Name, string? Description, string? Notes);