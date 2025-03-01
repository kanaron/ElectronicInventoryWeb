using Domain.Data;
using Domain.Dto;
using Domain.TmeModels;
using System.Globalization;

namespace Domain.Mappers;

public static class InventoryMappers
{
    public static InventoryItemDto ToInventoryItemDto(this InventoryItem item)
    {
        return new InventoryItemDto
        {
            Id = item.Id,
            Type = item.Type,
            Symbol = item.Symbol,
            Category = item.Category,
            Value = item.Value,
            StandardUnit = item.StandardUnit,
            StandardValue = item.StandardValue,
            Package = item.Package,
            Quantity = item.Quantity,
            Location = item.Location,
            DatasheetLink = item.DatasheetLink,
            StoreLink = item.StoreLink,
            PhotoUrl = item.PhotoUrl,
            MinStockLevel = item.MinStockLevel,
            Description = item.Description,
            IsActive = item.IsActive,
            Tags = item.Tags,
            DateAdded = item.DateAdded,
            LastUpdated = item.LastUpdated
        };
    }

    public static InventoryItem ToInventoryItem(this InventoryItemDto item)
    {
        return new InventoryItem
        {
            Id = item.Id,
            Type = item.Type,
            Symbol = item.Symbol,
            Category = item.Category,
            Value = item.Value,
            StandardUnit = item.StandardUnit,
            StandardValue = item.StandardValue,
            Package = item.Package,
            Quantity = item.Quantity,
            Location = item.Location,
            DatasheetLink = item.DatasheetLink,
            StoreLink = item.StoreLink,
            PhotoUrl = item.PhotoUrl,
            MinStockLevel = item.MinStockLevel,
            Description = item.Description,
            IsActive = item.IsActive,
            Tags = item.Tags,
            DateAdded = item.DateAdded,
            LastUpdated = item.LastUpdated
        };
    }

    public static UpdateInventoryItemDto ToUpdateInventoryItemDto(this InventoryItem item)
    {
        return new UpdateInventoryItemDto
        {
            Id = item.Id,
            Type = item.Type,
            Symbol = item.Symbol,
            Category = item.Category,
            Value = item.Value,
            StandardUnit = item.StandardUnit,
            StandardValue = item.StandardValue,
            Package = item.Package,
            Quantity = item.Quantity,
            Location = item.Location,
            DatasheetLink = item.DatasheetLink,
            StoreLink = item.StoreLink,
            PhotoUrl = item.PhotoUrl,
            MinStockLevel = item.MinStockLevel,
            Description = item.Description,
            IsActive = item.IsActive,
            Tags = item.Tags,
            LastUpdated = DateTime.Now
        };
    }

    public static InventoryItemDto FromTmeToInventoryItemDto(ProductWithDescription descriptionItem, ProductWithParameters parametersItem)
    {
        var (standardValue, standardUnit, rawValue) = GetValueFromParameters(parametersItem);

        return new InventoryItemDto
        {
            Type = descriptionItem.Category ?? string.Empty,
            Symbol = descriptionItem.Symbol!,
            Category = ExtractCategory(descriptionItem.Category),
            Value = rawValue, 
            StandardValue = standardValue, 
            StandardUnit = standardUnit, 
            Package = GetPackageFromParameters(parametersItem),
            Quantity = 0,
            DatasheetLink = string.Empty,
            StoreLink = descriptionItem.ProductInformationPage ?? string.Empty,
            PhotoUrl = descriptionItem.Photo,
            Description = descriptionItem.Description ?? string.Empty,
            Tags = new[] {
                parametersItem.ParameterList?.FirstOrDefault(p => p.ParameterName.ToLower().Equals("mounting"))?.ParameterValue ?? string.Empty
            }
        };
    }

    private static string GetPackageFromParameters(ProductWithParameters parametersItem)
    {
        var packageParameterNames = new List<string>
    {
        "Case",
        "Case - inch",
        "Body dimensions",
        "Package"
    };

        var packageParameter = parametersItem.ParameterList?
            .FirstOrDefault(p => packageParameterNames.Contains(p.ParameterName, StringComparer.OrdinalIgnoreCase));

        return packageParameter?.ParameterValue ?? "Unknown";
    }

    private static (double StandardValue, string StandardUnit, string RawValue) GetValueFromParameters(ProductWithParameters parametersItem)
    {
        var prioritizedParameterNames = new List<string>
    {
        "Resistance", "Capacitance", "Inductance", "Output voltage", "LED colour"
    };

        foreach (var parameterGroup in prioritizedParameterNames)
        {
            var matchedParameter = parametersItem.ParameterList?
                .FirstOrDefault(p => parameterGroup.Equals(p.ParameterName));

            if (matchedParameter != null)
            {
                return NormalizeComponentValue(matchedParameter.ParameterValue ?? "Unknown");
            }
        }

        return (0, "", "N/A");
    }

    private static string ExtractCategory(string? fullCategory)
    {
        if (string.IsNullOrEmpty(fullCategory))
            return "Unknown";

        var categoryWords = fullCategory.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (capacitorUnpolarizedKeywords.Any(x => categoryWords.Contains(x)))
        {
            return "Unpolarized capacitor";
        }
        else if (capacitorPolarizedKeywords.Any(x => categoryWords.Contains(x)))
        {
            return "Polarized capacitor";
        }

        foreach (var word in categoryWords)
        {
            if (CategoryMappings.TryGetValue(word, out var simplifiedCategory))
            {
                return simplifiedCategory;
            }
        }

        return "Other";
    }

    public static (double StandardValue, string StandardUnit, string RawValue) NormalizeComponentValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return (0, "", value);

        var unitMap = new Dictionary<string, double>
    {
        { "p", 1e-12 }, { "n", 1e-9 }, { "u", 1e-6 }, { "m", 1e-3 },
        { "k", 1e3 }, { "M", 1e6 }, { "G", 1e9 }
    };

        var match = System.Text.RegularExpressions.Regex.Match(value, @"([0-9.]+)\s*([pnumkMG]?)\s*(Ω|Ohm|F|H)?", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (!match.Success)
            return (0, "", value);

        double numericValue = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        string prefix = match.Groups[2].Value;
        string unit = match.Groups[3].Value ?? "";

        if (unit.Equals("Ohm", StringComparison.OrdinalIgnoreCase))
            unit = "Ω";

        if (unitMap.ContainsKey(prefix))
            numericValue *= unitMap[prefix];

        return (numericValue, unit, value);
    }

    private static readonly Dictionary<string, string> CategoryMappings = new()
{
    { "capacitors", "Capacitor" },
    { "resistors", "Resistor" },
    { "inductors", "Inductor" },
    { "transistors", "Transistor" },
    { "STM32", "STM32" }
};

    private static readonly string[] capacitorUnpolarizedKeywords = new[] { "mlcc", "film", "ceramic" };
    private static readonly string[] capacitorPolarizedKeywords = new[] { "electrolytic", "tantalum", "polymer", "hybrid" };
}
