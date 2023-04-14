using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Tyne.AspNetCore;

public sealed class EnvironmentSpecificStaticFileOptions
{
    public string? RequestPattern { get; set; }

    public string? NewPathTemplate { get; set; }

    internal EnvironmentSpecificStaticFileOptions() { }

    public EnvironmentSpecificStaticFileOptions WithRequestPattern(string requestPattern)
    {
        RequestPattern = requestPattern;
        return this;
    }

    public EnvironmentSpecificStaticFileOptions WithNewPathTemplate(string newPathTemplate)
    {
        NewPathTemplate = newPathTemplate;
        return this;
    }

    [MemberNotNull(nameof(RequestPattern), nameof(NewPathTemplate))]
    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(RequestPattern))
            throw new InvalidOperationException($"Can't map an empty {nameof(RequestPattern)}.");

        if (string.IsNullOrWhiteSpace(NewPathTemplate))
            throw new InvalidOperationException($"Invalid template {nameof(NewPathTemplate)}.");

        try
        {
            _ = string.Format(CultureInfo.InvariantCulture, NewPathTemplate, "Validation");
        }
        catch (FormatException formatException)
        {
            throw new InvalidOperationException($"Invalid {nameof(NewPathTemplate)}: could not format template. See inner exception.", formatException);
        }
    }
}
