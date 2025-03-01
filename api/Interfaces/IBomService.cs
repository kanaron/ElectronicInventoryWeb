using Domain.Data;

namespace API.Interfaces;

public interface IBomService
{
    Task<Project> ProcessBomFileAsync(IFormFile file, string projectName, string category = "Default", string? description = null);
}