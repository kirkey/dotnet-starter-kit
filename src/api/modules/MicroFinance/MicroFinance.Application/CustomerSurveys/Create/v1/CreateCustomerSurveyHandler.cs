using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerSurveys.Create.v1;

/// <summary>
/// Handler for creating a new customer survey.
/// </summary>
public sealed class CreateCustomerSurveyHandler(
    ILogger<CreateCustomerSurveyHandler> logger,
    [FromKeyedServices("microfinance:customersurveys")] IRepository<CustomerSurvey> repository)
    : IRequestHandler<CreateCustomerSurveyCommand, CreateCustomerSurveyResponse>
{
    public async Task<CreateCustomerSurveyResponse> Handle(CreateCustomerSurveyCommand request, CancellationToken cancellationToken)
    {
        var survey = CustomerSurvey.Create(
            request.Title,
            request.SurveyType,
            request.StartDate,
            request.Description,
            request.EndDate,
            request.IsAnonymous);

        await repository.AddAsync(survey, cancellationToken);
        logger.LogInformation("Customer survey '{Title}' created with ID {Id}", survey.Title, survey.Id);

        return new CreateCustomerSurveyResponse(survey.Id, survey.Title, survey.Status);
    }
}
