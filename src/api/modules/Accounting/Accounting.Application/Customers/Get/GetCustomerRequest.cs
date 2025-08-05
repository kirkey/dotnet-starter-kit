using MediatR;
using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Get;

public record GetCustomerRequest(DefaultIdType Id) : IRequest<CustomerDto>;
