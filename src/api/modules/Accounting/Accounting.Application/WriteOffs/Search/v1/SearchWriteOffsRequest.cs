using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Application.WriteOffs.Search.v1;

/// <summary>
/// Request to search for write-offs with optional filters.
/// </summary>
public record SearchWriteOffsRequest(
    string? ReferenceNumber = null,
    DefaultIdType? CustomerId = null,
    string? WriteOffType = null,
    string? Status = null,
    bool? IsRecovered = null) : IRequest<List<WriteOffResponse>>;

