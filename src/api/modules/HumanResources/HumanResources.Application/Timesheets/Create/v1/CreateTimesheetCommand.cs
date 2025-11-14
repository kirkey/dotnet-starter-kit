namespace FSH.Starter.WebApi.HumanResources.Application.Timesheets.Create.v1;

public sealed record CreateTimesheetCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("2025-11-10")] DateTime StartDate,
    [property: DefaultValue("2025-11-16")] DateTime EndDate,
    [property: DefaultValue("Weekly")] string PeriodType = "Weekly") : IRequest<CreateTimesheetResponse>;
