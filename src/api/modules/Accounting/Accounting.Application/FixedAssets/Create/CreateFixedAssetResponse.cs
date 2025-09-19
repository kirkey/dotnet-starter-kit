namespace Accounting.Application.FixedAssets.Create;

/// <summary>
/// Response returned after creating a Fixed Asset, containing the new Asset Id.
/// </summary>
/// <param name="Id">Identifier of the created Fixed Asset.</param>
public sealed record CreateFixedAssetResponse(DefaultIdType Id);

