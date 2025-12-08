using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Delete.v1;

public sealed record DeleteReportDefinitionCommand(DefaultIdType Id) : IRequest;

