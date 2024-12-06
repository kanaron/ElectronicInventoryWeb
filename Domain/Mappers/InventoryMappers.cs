using Domain.Data;
using Domain.Dto;
using Domain.TmeModels;

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

    public static InventoryItemDto FromTmeToInventoryItemDto(ProductWithDescription descriptionItem, ProductWithParameters parametersItem)
    {
        return new InventoryItemDto
        {
            Type = descriptionItem.Category ?? string.Empty,
            Symbol = descriptionItem.Symbol!,
            Category = descriptionItem.Category ?? string.Empty,
            Value = GetValueFromParameters(parametersItem),
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

    private static string GetValueFromParameters(ProductWithParameters parametersItem)
    {
        var prioritizedParameterNames = new List<string>
    {
        "Resistance", "Capacitance", "Inductance", "Output voltage"
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

        return "N/A";
    }
}
