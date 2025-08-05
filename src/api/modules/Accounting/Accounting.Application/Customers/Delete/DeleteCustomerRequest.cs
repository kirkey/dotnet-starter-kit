using MediatR;

namespace Accounting.Application.Customers.Delete;

public record DeleteCustomerRequest(DefaultIdType Id) : IRequest;
