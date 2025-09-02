namespace Accounting.Domain.Exceptions;

public class RegulatoryReportInvalidException : Exception
{
    public RegulatoryReportInvalidException(string message) : base(message)
    {
    }
}
