using Accounting.Application.Checks.Exceptions;

namespace Accounting.Application.Checks.Print.v1;

/// <summary>
/// Handler for marking a check as printed.
/// </summary>
public sealed class PrintCheckHandler(
    ILogger<PrintCheckHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<PrintCheckCommand, CheckPrintResponse>
{
    public async Task<CheckPrintResponse> Handle(PrintCheckCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var check = await repository.GetByIdAsync(request.CheckId, cancellationToken);
        if (check == null)
        {
            throw new CheckNotFoundException(request.CheckId);
        }

        check.MarkAsPrinted(request.PrintedBy, request.PrintedDate);

        await repository.UpdateAsync(check, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Check marked as printed: {CheckId} - {CheckNumber} by {PrintedBy}", 
            check.Id, check.CheckNumber, request.PrintedBy);

        return new CheckPrintResponse(check.Id, check.CheckNumber, check.IsPrinted);
    }
}
