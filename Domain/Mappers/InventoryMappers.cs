using Domain.Data;
using Domain.Dto;
using Domain.TmeModels;
using System.Globalization;
using System.Text.RegularExpressions;

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
            Package = item.Package,
            Quantity = item.Quantity,
            ReservedForProjects = item.ReservedForProjects,
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
        var (standardValue, standardUnit, rawValue) = NormalizeComponentValue(item.Value);

        return new InventoryItem
        {
            Id = item.Id,
            Type = item.Type,
            Symbol = item.Symbol,
            Category = item.Category,
            Value = rawValue,
            StandardUnit = standardUnit,
            StandardValue = standardValue,
            Package = item.Package,
            Quantity = item.Quantity,
            ReservedForProjects = item.ReservedForProjects,
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
            ReservedForProjects = item.ReservedForProjects,
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
        return new InventoryItemDto
        {
            Type = descriptionItem.Category ?? string.Empty,
            Symbol = descriptionItem.Symbol!,
            Category = ExtractCategory(descriptionItem.Category),
            Value = GetValueFromParameters(parametersItem), 
            Package = GetPackageFromParameters(parametersItem),
            Quantity = 0,
            ReservedForProjects = 0,
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

    private static string GetValueFromParameters(ProductWithParameters parametersItem)
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
                return matchedParameter.ParameterValue ?? "Unknown";
            }
        }

        return ("N/A");
    }

    private static string ExtractCategory(string? fullCategory)
    {
        if (string.IsNullOrWhiteSpace(fullCategory))
            return "Unknown";

        var categoryWords = fullCategory.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (categoryWords.Any(word => word.Contains("led")))
            return "Light emitting diode";

        if (categoryWords.Contains("diode") || categoryWords.Contains("zener"))
            return "Diode";

        if (categoryWords.Contains("mosfet"))
            return "MOSFET";

        if (categoryWords.Contains("transistor"))
            return "Transistor";

        if (categoryWords.Contains("regulator"))
            return "Voltage regulator";

        if (categoryWords.Contains("logic") || categoryWords.Contains("gate"))
            return "Logic IC";

        if (capacitorUnpolarizedKeywords.Any(x => categoryWords.Contains(x)))
            return "Unpolarized capacitor";

        if (capacitorPolarizedKeywords.Any(x => categoryWords.Contains(x)))
            return "Polarized capacitor";

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

        value = value.Replace('µ', 'u'); // Normalize micro sign

        var unitMap = new Dictionary<string, double>
    {
        { "p", 1e-12 }, { "n", 1e-9 }, { "u", 1e-6 }, { "m", 1e-3 },
        { "k", 1e3 }, { "M", 1e6 }, { "G", 1e9 }, { "R", 1 } // R is for ohms
    };

        // 1. Try to match IEC-style values like 22k1, 4R7, 1n2
        var iecMatch = Regex.Match(value, @"^(\d+)([pnumkMGRR])(\d*)(Ω|Ohm|F|H)?$", RegexOptions.IgnoreCase);
        if (iecMatch.Success)
        {
            string leading = iecMatch.Groups[1].Value;
            string _multiplier = iecMatch.Groups[2].Value.ToLower(); // lowercase for dictionary
            string trailing = iecMatch.Groups[3].Value;
            string unit = iecMatch.Groups[4].Value ?? "";

            if (unit.Equals("Ohm", StringComparison.OrdinalIgnoreCase))
                unit = "Ω";

            if (unitMap.TryGetValue(_multiplier, out double factor))
            {
                string numeric = $"{leading}.{trailing}";
                if (double.TryParse(numeric, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                {
                    return (parsedValue * factor, unit, value);
                }
            }
        }

        // 2. Fallback: classic regex with space
        var match = Regex.Match(value, @"([0-9.,]+)\s*([pnumkMG]?)\s*(Ω|Ohm|F|H)?", RegexOptions.IgnoreCase);

        if (!match.Success)
            return (0, "", value);

        string number = match.Groups[1].Value.Replace(",", "").Trim();
        string prefix = match.Groups[2].Value;
        string unitSymbol = match.Groups[3].Value ?? "";

        if (unitSymbol.Equals("Ohm", StringComparison.OrdinalIgnoreCase))
            unitSymbol = "Ω";

        if (!double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out double baseValue))
            return (0, "", value);

        if (unitMap.TryGetValue(prefix.ToLower(), out double multiplier))
            baseValue *= multiplier;

        return (baseValue, unitSymbol, value);
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
