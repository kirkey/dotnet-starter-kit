using Accounting.Application.Meters.Responses;

namespace Accounting.Application.Meters.Get.v1;

/// <summary>
/// Request to get a meter by ID.
/// </summary>
public sealed record GetMeterRequest(DefaultIdType Id) : IRequest<MeterResponse>;

