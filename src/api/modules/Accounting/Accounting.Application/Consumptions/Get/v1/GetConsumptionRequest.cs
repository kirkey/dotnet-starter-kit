using Accounting.Application.Consumptions.Responses;
namespace Accounting.Application.Consumptions.Get.v1;
/// <summary>
/// Request to get a consumption record by ID.
/// </summary>
public sealed record GetConsumptionRequest(DefaultIdType Id) : IRequest<ConsumptionResponse>;
