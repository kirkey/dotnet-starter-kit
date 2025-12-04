using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;

/// <summary>
/// Request to get a customer survey by ID.
/// </summary>
public sealed record GetCustomerSurveyRequest(Guid Id) : IRequest<CustomerSurveyResponse>;
