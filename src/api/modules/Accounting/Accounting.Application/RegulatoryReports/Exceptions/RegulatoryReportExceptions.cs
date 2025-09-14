namespace Accounting.Application.RegulatoryReports.Exceptions;

public class RegulatoryReportForbiddenException : ForbiddenException
{
    public RegulatoryReportForbiddenException(string message) : base(message)
    {
    }
}

public class RegulatoryReportNotFoundException : NotFoundException
{
    public RegulatoryReportNotFoundException(DefaultIdType id) : base($"Regulatory report with ID {id} was not found")
    {
    }
}
