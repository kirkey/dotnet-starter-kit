using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Get.v1;

/// <summary>
/// Request to get a customer case by ID.
/// </summary>
public sealed record GetCustomerCaseRequest(Guid Id) : IRequest<CustomerCaseResponse>;
