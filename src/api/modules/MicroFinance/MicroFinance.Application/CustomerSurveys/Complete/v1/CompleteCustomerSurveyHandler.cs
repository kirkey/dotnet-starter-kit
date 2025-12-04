using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Complete.v1;

/// <summary>
/// Handler to complete a customer survey.
/// </summary>
public sealed class CompleteCustomerSurveyHandler(
    ILogger<CompleteCustomerSurveyHandler> logger,
    [FromKeyedServices("microfinance:customersurveys")] IRepository<CustomerSurvey> repository)
    : IRequestHandler<CompleteCustomerSurveyCommand, CompleteCustomerSurveyResponse>
{
    public async Task<CompleteCustomerSurveyResponse> Handle(CompleteCustomerSurveyCommand request, CancellationToken cancellationToken)
    {
        var survey = await repository.FirstOrDefaultAsync(new CustomerSurveyByIdSpec(request.Id), cancellationToken)
            ?? throw new Exception($"Customer survey {request.Id} not found");

        survey.Complete();
        await repository.UpdateAsync(survey, cancellationToken);

        logger.LogInformation("Customer survey {Id} completed with {Responses} responses", survey.Id, survey.TotalResponses);

        return new CompleteCustomerSurveyResponse(survey.Id, survey.Status, survey.TotalResponses, survey.AverageScore);
    }
}
