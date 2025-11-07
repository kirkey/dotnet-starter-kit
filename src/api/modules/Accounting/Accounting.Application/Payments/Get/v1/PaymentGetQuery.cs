namespace Accounting.Application.Payments.Get.v1;

/// <summary>
/// Query to retrieve a payment by its unique identifier.
/// </summary>
public sealed record PaymentGetQuery(DefaultIdType Id) : IRequest<PaymentGetResponse>;

