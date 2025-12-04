using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;

/// <summary>
/// Handler to get a customer survey by ID.
/// </summary>
public sealed class GetCustomerSurveyHandler(
    ILogger<GetCustomerSurveyHandler> logger,
    [FromKeyedServices("microfinance:customersurveys")] IReadRepository<CustomerSurvey> repository)
    : IRequestHandler<GetCustomerSurveyRequest, CustomerSurveyResponse>
{
    public async Task<CustomerSurveyResponse> Handle(GetCustomerSurveyRequest request, CancellationToken cancellationToken)
    {
        var survey = await repository.FirstOrDefaultAsync(new CustomerSurveyByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Customer survey {request.Id} not found");

        logger.LogInformation("Retrieved customer survey {Id}", survey.Id);

        return new CustomerSurveyResponse(
            survey.Id,
            survey.Title,
            survey.Description,
            survey.SurveyType,
            survey.Status,
            survey.StartDate,
            survey.EndDate,
            survey.TotalResponses,
            survey.AverageScore,
            survey.NpsScore,
            survey.IsAnonymous);
    }
}
