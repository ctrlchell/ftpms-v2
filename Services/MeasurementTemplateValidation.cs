using ftpms.ViewModels;

namespace ftpms.Services;

public static class MeasurementTemplateValidation
{
    private static readonly Dictionary<string, (Func<MeasurementInputViewModel, decimal?> Selector, string Label)[]> Rules =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Suit"] =
            [
                (m => m.Chest, "Chest"),
                (m => m.Shoulder, "Shoulder"),
                (m => m.SleeveLength, "SleeveLength"),
                (m => m.TopLength, "TopLength")
            ],
            ["Shirt"] =
            [
                (m => m.Chest, "Chest"),
                (m => m.Shoulder, "Shoulder"),
                (m => m.SleeveLength, "SleeveLength"),
                (m => m.TopLength, "TopLength")
            ],
            ["Trouser"] =
            [
                (m => m.Waist, "Waist"),
                (m => m.Hip, "Hip"),
                (m => m.TrouserLength, "TrouserLength")
            ],
            ["Gown"] =
            [
                (m => m.Chest, "Chest"),
                (m => m.Waist, "Waist"),
                (m => m.Hip, "Hip"),
                (m => m.GownLength, "GownLength")
            ],
            ["Skirt"] =
            [
                (m => m.Waist, "Waist"),
                (m => m.Hip, "Hip"),
                (m => m.SkirtLength, "SkirtLength")
            ],
            ["Agbada"] =
            [
                (m => m.Chest, "Chest"),
                (m => m.Shoulder, "Shoulder"),
                (m => m.SleeveLength, "SleeveLength"),
                (m => m.TopLength, "TopLength")
            ]
        };

    public static Dictionary<string, List<string>> Validate(MeasurementInputViewModel input)
    {
        var errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrWhiteSpace(input.TemplateType) || !Rules.TryGetValue(input.TemplateType, out var ruleSet))
        {
            return errors;
        }

        foreach (var (selector, label) in ruleSet)
        {
            if (!selector(input).HasValue)
            {
                AddError(errors, label, $"{label} is required for template '{input.TemplateType}'.");
            }
        }

        return errors;
    }

    private static void AddError(IDictionary<string, List<string>> errors, string key, string message)
    {
        if (!errors.TryGetValue(key, out var value))
        {
            value = [];
            errors[key] = value;
        }

        value.Add(message);
    }
}
