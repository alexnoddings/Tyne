namespace Tyne.Blazor.Filtering.Values;

public static class FilterSelectItem
{
    public static FilterSelectItem<TValue> Create<TValue>(TValue? value, string asString) =>
        new FilterSelectItem<TValue>(value, asString);

    public static FilterSelectItem<TValue, TMetadata> Create<TValue, TMetadata>(TValue? value, string asString, TMetadata? metadata) =>
        new FilterSelectItem<TValue, TMetadata>(value, asString, metadata);
}
