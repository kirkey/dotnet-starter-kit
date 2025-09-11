using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Customers.Update.v1;

public sealed record UpdateCustomerCommand(
    DefaultIdType Id,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Email,
    string? Address,
    DateTime? DateOfBirth,
    decimal? LoyaltyPoints,
    decimal? TotalSpent) : IRequest<UpdateCustomerResponse>;

