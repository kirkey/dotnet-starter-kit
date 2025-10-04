namespace FSH.Starter.WebApi.Store.Application.Bins.Update.v1;

/// <summary>
/// Response returned after updating a bin.
/// </summary>
/// <param name="Id">The identifier of the updated bin.</param>
public record UpdateBinResponse(DefaultIdType Id);

