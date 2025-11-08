namespace Accounting.Application.WriteOffs.Post.v1;

public sealed class PostWriteOffCommandValidator : AbstractValidator<PostWriteOffCommand>
{
    public PostWriteOffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Write-off ID is required.");
        RuleFor(x => x.JournalEntryId).NotEmpty().WithMessage("Journal entry ID is required.");
    }
}

