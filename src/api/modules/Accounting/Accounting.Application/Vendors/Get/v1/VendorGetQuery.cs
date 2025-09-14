namespace Accounting.Application.Vendors.Get.v1;

public record VendorGetQuery(DefaultIdType Id) : IRequest<VendorGetResponse>;

