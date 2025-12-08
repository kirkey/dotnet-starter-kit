using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Update.v1;

public sealed record UpdateAgentBankingCommand(
    DefaultIdType Id,
    string? BusinessName = null,
    string? ContactName = null,
    string? PhoneNumber = null,
    string? Email = null,
    string? Address = null,
    string? GpsCoordinates = null,
    string? OperatingHours = null,
    decimal? CommissionRate = null,
    decimal? DailyTransactionLimit = null,
    decimal? MonthlyTransactionLimit = null) : IRequest<UpdateAgentBankingResponse>;
