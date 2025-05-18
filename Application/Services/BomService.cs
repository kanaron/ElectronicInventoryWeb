using Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Data;
using Domain.Dto;
using Domain.Mappers;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Services;

public class BomService : IBomService
{
    public async Task<Project> ProcessBomFileAsync(IFormFile file, string projectName, string category = "Default", string? description = null)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file.");

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = projectName,
            Category = category,
            Description = description,
            BomItems = []
        };

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        string extension = Path.GetExtension(file.FileName).ToLower();
        if (extension == ".csv")
        {
            project.BomItems = ParseCsv(stream);
        }
        else
        {
            throw new NotSupportedException("Only CSV files are supported.");
        }

        return project;
    }

    private List<BomItem> ParseCsv(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<BomItemCsvMap>();

        var records = csv.GetRecords<BomItemDto>().ToList();

        return records.Select(r =>
        {
            var (standardValue, standardUnit, rawValue) = InventoryMappers.NormalizeComponentValue(r.Value);

            var autoCategory = r.Category;

            if (!string.IsNullOrWhiteSpace(autoCategory))
            {
                var lowered = autoCategory.ToLowerInvariant();

                if (lowered.Contains("mosfet") || lowered.Contains("transistor") || lowered.Contains("fet"))
                {
                    autoCategory = "Transistor";
                }
            }

            return new BomItem
            {
                Id = Guid.NewGuid(),
                Category = autoCategory,
                Value = rawValue,
                StandardValue = standardValue,
                StandardUnit = standardUnit,
                Package = NormalizeBomFootprint(r.Package, r.References![0]),
                References = r.References,
                Quantity = r.Quantity,
                Description = r.Description,
                IsRelevant = true,
                IsPlaced = false
            };
        }).ToList();
    }

    private static string NormalizeBomFootprint(string footprint, char reference)
    {
        if (string.IsNullOrWhiteSpace(footprint))
            return "Unknown";

        if (reference == 'J')
            return footprint;

        if (reference == 'U' || reference == 'Q')
        {
            var match = Regex.Match(footprint, @"Package_[\w_]+:(\w+-\d+)");
            if (match.Success)
                return match.Groups[1].Value;

            match = Regex.Match(footprint, @"(\d+(?:\.\d+)?)x(\d+(?:\.\d+)?)mm");
            if (match.Success)
                return $"{match.Groups[1].Value}x{match.Groups[2].Value}mm";
        }

        var smdMatch = Regex.Match(footprint, @"_(\d{4})_\d+Metric");
        if (smdMatch.Success)
            return smdMatch.Groups[1].Value;

        var radialMatch = Regex.Match(footprint, @"_D(\d+\.?\d*)mm_P(\d+\.?\d*)mm");
        if (radialMatch.Success)
            return $"Ø{radialMatch.Groups[1].Value}x{radialMatch.Groups[2].Value}mm";

        var genericSizeMatch = Regex.Match(footprint, @"_([\d.]+x[\d.]+)");
        if (genericSizeMatch.Success)
            return genericSizeMatch.Groups[1].Value;

        return footprint;
    }
}

public class BomItemCsvMap : ClassMap<BomItemDto>
{
    public BomItemCsvMap()
    {
        Map(m => m.Category).Name("Description");
        Map(m => m.Value).Name("Value");
        Map(m => m.Package).Name("Footprint");
        Map(m => m.References).Name("Reference");
        Map(m => m.Quantity).Name("Qty");
        Map(m => m.Description).Name("Description");
    }
}
