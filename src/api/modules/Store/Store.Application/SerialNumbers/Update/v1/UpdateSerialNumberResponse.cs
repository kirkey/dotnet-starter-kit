namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

/// <summary>
/// Response returned after updating a serial number.
/// </summary>
/// <param name="Id">The identifier of the updated serial number.</param>
public record UpdateSerialNumberResponse(DefaultIdType Id);

