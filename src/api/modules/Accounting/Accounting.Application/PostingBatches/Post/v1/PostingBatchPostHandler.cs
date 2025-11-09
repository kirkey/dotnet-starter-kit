using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.PostingBatches.Post.v1;

public sealed class PostingBatchPostHandler(
    IRepository<PostingBatch> repository,
    ICurrentUser currentUser,
    ILogger<PostingBatchPostHandler> logger)
    : IRequestHandler<PostingBatchPostCommand, DefaultIdType>
{
    private readonly IRepository<PostingBatch> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICurrentUser _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
    private readonly ILogger<PostingBatchPostHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(PostingBatchPostCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Posting batch {BatchId}", request.Id);

        var batch = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (batch == null) throw new NotFoundException($"Posting batch with ID {request.Id} not found");

        var postedBy = _currentUser.GetUserName() ?? _currentUser.Name ?? "System";
        batch.Post(postedBy);
        await _repository.UpdateAsync(batch, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Posting batch {BatchNumber} posted successfully by {User}", batch.BatchNumber, postedBy);
        return batch.Id;
    }
}

