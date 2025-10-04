namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;

/// <summary>
/// Response returned after updating a lot number.
/// </summary>
/// <param name="Id">The identifier of the updated lot number.</param>
public record UpdateLotNumberResponse(DefaultIdType Id);

