namespace Accounting.Application.Projects.Specifications;

/// <summary>
/// Specification to find a project by its name for duplicate checking.
/// </summary>
public class ProjectByNameSpec : Specification<Project>
{
    public ProjectByNameSpec(string name) => Query.Where(p => p.Name == name);
}
