using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Activate.v1;

/// <summary>
/// Handler to activate a customer survey.
/// </summary>
public sealed class ActivateCustomerSurveyHandler(
    ILogger<ActivateCustomerSurveyHandler> logger,
    [FromKeyedServices("microfinance:customersurveys")] IRepository<CustomerSurvey> repository)
    : IRequestHandler<ActivateCustomerSurveyCommand, ActivateCustomerSurveyResponse>
{
    public async Task<ActivateCustomerSurveyResponse> Handle(ActivateCustomerSurveyCommand request, CancellationToken cancellationToken)
    {
        var survey = await repository.FirstOrDefaultAsync(new CustomerSurveyByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Customer survey {request.Id} not found");

        survey.Activate();
        await repository.UpdateAsync(survey, cancellationToken);

        logger.LogInformation("Customer survey {Id} activated", survey.Id);

        return new ActivateCustomerSurveyResponse(survey.Id, survey.Status);
    }
}
