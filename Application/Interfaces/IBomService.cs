using Domain.Data;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IBomService
{
    Task<Project> ProcessBomFileAsync(IFormFile file, string projectName, string category = "Default", string? description = null);
}