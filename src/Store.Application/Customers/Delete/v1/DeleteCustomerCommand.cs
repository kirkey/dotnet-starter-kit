namespace FSH.Starter.WebApi.Store.Application.Customers.Delete.v1;

public sealed record DeleteCustomerCommand(
    DefaultIdType Id) : IRequest;
