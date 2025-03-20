using MediatR;

namespace FSH.Framework.Core.Storage.File.Features;

public class FileUploadCommand : IRequest<FileUploadResponse>
{
    public string Name { get; set; } = null!;
    public string Extension { get; set; } = null!;
    public string Data { get; set; } = null!;
}
