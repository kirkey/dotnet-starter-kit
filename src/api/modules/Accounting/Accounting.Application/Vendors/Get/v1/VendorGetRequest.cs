namespace Accounting.Application.Vendors.Get.v1;

/// <summary>
/// Request to retrieve a vendor by ID.
/// </summary>
public record VendorGetRequest(DefaultIdType Id) : IRequest<VendorGetResponse>;
