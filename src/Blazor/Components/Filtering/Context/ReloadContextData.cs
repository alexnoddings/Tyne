namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     A delegate which will trigger data to be reloaded
///     when an <see cref="IFilterContext{TRequest}"/> is updated.
/// </summary>
/// <returns>A <see cref="Task"/> representing the data being asynchronously reloaded.</returns>
public delegate Task ReloadContextData();
