using Accounting.Application.Projects.Queries;
using Accounting.Domain.Entities;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;

namespace Accounting.Application.Projects.Update.v1;

/// <summary>
/// Handler for updating existing projects with proper domain validation and event publishing.
/// </summary>
public sealed class UpdateProjectHandler(
    ILogger<UpdateProjectHandler> logger,
    [FromKeyedServices("accounting:projects")] IRepository<Project> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    /// <summary>
    /// Handles the update of an existing project entity.
    /// If the client uploaded an image, saves it to storage and sets ImageUrl to the returned public URI.
    /// </summary>
    /// <param name="request">The update project command containing all updated information.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Response indicating successful update.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="ProjectNotFoundException">Thrown when the project is not found.</exception>
    public async Task<UpdateProjectResponse> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get the existing project
        var project = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectNotFoundException(request.Id);

        // Check for duplicate name if name is being changed
        if (!string.IsNullOrWhiteSpace(request.Name) && project.Name != request.Name)
        {
            var existingProject = await repository.FirstOrDefaultAsync(
                new ProjectByNameSpec(request.Name), cancellationToken).ConfigureAwait(false);
            
            if (existingProject is not null)
            {
                throw new DuplicateProjectException(request.Name);
            }
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

        // Update the project using domain method
        project.Update(request.Name,
            request.StartDate,
            request.EndDate,
            request.BudgetedAmount,
            request.Status,
            request.ClientName,
            request.ProjectManager,
            request.Department,
            request.Description,
            request.Notes,
            imageUrl);

        // Save changes
        await repository.UpdateAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("project updated {ProjectId}. ImageUrl: {ImageUrl}", project.Id, imageUrl);

        return new UpdateProjectResponse(project.Id);
    }
}
