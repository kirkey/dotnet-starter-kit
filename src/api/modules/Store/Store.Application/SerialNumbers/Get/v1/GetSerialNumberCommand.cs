namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

public sealed record GetSerialNumberCommand : IRequest<SerialNumberResponse>
{
    public DefaultIdType Id { get; set; }
}
