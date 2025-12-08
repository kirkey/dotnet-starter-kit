using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.AgentBankings.Create.v1;

public sealed record CreateAgentBankingCommand(
    string AgentCode,
    string BusinessName,
    string ContactName,
    string PhoneNumber,
    string Address,
    decimal CommissionRate,
    decimal DailyTransactionLimit,
    decimal MonthlyTransactionLimit,
    DateOnly ContractStartDate,
    DefaultIdType? BranchId = null,
    string? Email = null,
    string? GpsCoordinates = null,
    string? OperatingHours = null) : IRequest<CreateAgentBankingResponse>;
