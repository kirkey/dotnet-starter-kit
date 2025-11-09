using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.WriteOffs.Approve.v1;

public sealed class ApproveWriteOffHandler(
    ICurrentUser currentUser,
    IRepository<WriteOff> repository, 
    ILogger<ApproveWriteOffHandler> logger)
    : IRequestHandler<ApproveWriteOffCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveWriteOffCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        logger.LogInformation("Approving write-off {Id}", request.Id);

        var writeOff = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (writeOff == null) throw new NotFoundException($"Write-off with ID {request.Id} not found");

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        writeOff.Approve(approverId, approverName);
        await repository.UpdateAsync(writeOff, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Write-off {ReferenceNumber} approved by user {ApproverId}", 
            writeOff.ReferenceNumber, approverId);
        return writeOff.Id;
    }
}

