using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Application.WriteOffs.Get;

/// <summary>
/// Request to get a write-off by ID.
/// </summary>
public record GetWriteOffRequest(DefaultIdType Id) : IRequest<WriteOffResponse>;
