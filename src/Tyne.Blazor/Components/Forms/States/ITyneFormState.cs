using System.Diagnostics.CodeAnalysis;

namespace Tyne.Blazor;

[SuppressMessage(
    "Design",
    "CA1040: Avoid empty interfaces.",
    Justification = "Base interface for other form states."
)]
public interface ITyneFormState;
