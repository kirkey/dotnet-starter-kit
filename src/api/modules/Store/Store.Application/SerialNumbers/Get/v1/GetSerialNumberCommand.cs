namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

/// <summary>
/// Command to get a serial number by ID.
/// </summary>
public sealed record GetSerialNumberCommand(DefaultIdType Id) : IRequest<SerialNumberResponse>;
