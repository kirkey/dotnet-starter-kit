namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Delete.v1;

public class DeleteSerialNumberCommand : IRequest<DeleteSerialNumberResponse>
{
    public DefaultIdType Id { get; set; }
}

public class DeleteSerialNumberResponse(DefaultIdType id, string serialNumberValue)
{
    public DefaultIdType Id { get; } = id;
    public string SerialNumberValue { get; } = serialNumberValue;
}
