using Accounting.Application.Projects.Queries;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace Accounting.Application.Projects.Create.v1;

/// <summary>
/// Handler for creating new projects with proper domain validation and event publishing.
/// </summary>
public sealed class CreateProjectHandler(
    ILogger<CreateProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository,
    IStorageService storageService)
    : IRequestHandler<CreateProjectCommand, CreateProjectResponse>
{
    /// <summary>
    /// Handles the creation of a new project entity.
    /// If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    /// <param name="request">The create project command containing all required information.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response containing the ID of the newly created project.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<CreateProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate project name
        var existingProject = await repository.FirstOrDefaultAsync(
            new ProjectByNameSpec(request.Name), cancellationToken).ConfigureAwait(false);
        
        if (existingProject is not null)
        {
            throw new DuplicateProjectException(request.Name);
        }

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null && !string.IsNullOrWhiteSpace(request.Image.Data))
        {
            var uri = await storageService.UploadAsync<Project>(request.Image, FileType.Image, cancellationToken).ConfigureAwait(false);
            if (uri is null)
            {
                throw new InvalidOperationException("Image upload failed: storage provider returned no URI.");
            }

            // Persist the full absolute URI returned by the storage provider so clients can load images directly.
            imageUrl = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }

        // Create the project using the domain factory method
        var project = Project.Create(
            request.Name,
            request.StartDate,
            request.BudgetedAmount,
            request.ClientName,
            request.ProjectManager,
            request.Department,
            request.Description,
            request.Notes,
            imageUrl);

        // Save to repository
        await repository.AddAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project created {ProjectId}. ImageUrl: {ImageUrl}", project.Id, imageUrl);

        return new CreateProjectResponse(project.Id);
    }
}
