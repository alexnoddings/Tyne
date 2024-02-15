---
title: Persistence
---

# Filter value persistence
Tyne provides client-side URL persistence, which is designed to:
- Allow users to share pages easier, such as a link to a table pre-configured with a date range
- Allow users to bookmark pages with specific searches, such as a table showing only data belonging to them
- Allow developers to link users to pages with pre-configured state, such as a table showing data belonging to a certain category


## URL persistence service
URL persistence is done through the [`IUrlPersistenceService`](xref:Tyne.Blazor.Persistence.IUrlPersistenceService).

### [GetValue<T>](xref:Tyne.Blazor.Persistence.IUrlPersistenceService.GetValue*)
Deserialises a `T` from the URL.

### [SetValue<T>](xref:Tyne.Blazor.Persistence.IUrlPersistenceService.SetValue*)
Serialises a `T` to the URL. Only one value should be set at a time using this method. See [bulk value setting](#bulk-value-setting) for why.

### [BulkSetValues](xref:Tyne.Blazor.Persistence.IUrlPersistenceService.BulkSetValues*)
Serialise multiple values to the URL.

#### [BulkSetValues(IReadOnlyDictionary<string, object>)](xref:Tyne.Blazor.Persistence.IUrlPersistenceService.BulkSetValues(System.Collections.Generic.IReadOnlyDictionary{System.String,System.Object}))
This overload takes a dictionary of `string, object` pairs. The `string` forms the parameter name in the URL, while the `object` is what is serialised to the URL.

#### [BulkSetValues(object)](xref:Tyne.Blazor.Persistence.IUrlPersistenceService.BulkSetValues(System.Object))
This overload takes an `object`, and reflects across it's public instanced members. The name of each forms the parameter name in the URL, while the value is what is serialised to the URL.

This is provided for easy usage with anonymous types, for example:
```cs
_urlPersistenceService.BulkSetValues(new {
    ParamA = "Hello, World!",
    ParamB = SomeEnum.Value,
    ParamC = 42,
    ParamD = (object?)null
});
```


## Bulk value setting
If persisting more than one value to the URL in a short span of time, `SetValue` is wholly unsuitable.
Blazor's [WebAssemblyNavigationManager implementation](https://github.com/dotnet/aspnetcore/blob/v8.0.1/src/Components/WebAssembly/WebAssembly/src/Services/WebAssemblyNavigationManager.cs#L59)
fires-and-forgets an async task to perform the navigation, then returns synchronously.
The knock-on effect is that multiple calls to `SetValue` in quick succession will spawn overlapping tasks, usually causing only one of the values provided to actually get set. The rest are lost to the race condition.

`BulkSetValues` is designed to update all of the values provided in one go.


## Formatting footnotes
Vales are serialised with custom rules. These are designed to keep the URL cleaner and shorter.

The rules used are:
- `string`s are trimmed, and empty strings are set as `null`
- `Guid`s are compacted
- `DateTime`s are converted to `yyyyMMddHHmmss`
- `Enum`s are converted to their named string counterpart
- Empty `ICollection`s are set as `null`
- As a fallback, values are JSON serialised

### GUID compactification
GUIDs in URLs are compacted to save space, cutting 32 chars down to 22.

GUIDs are 128 bit, and are normally formatted as 32 chars using a 16 character set (`[0-9a-f]`).
Tyne instead converts the GUID to base64, a 64 character set (`[a-zA-Z0-9@%]`).
Note that the last two characters are `@%` rather than `/+` to keep it URL friendly.

For example, the GUID `657db550-f8ad-418f-9927-5dc555258e78` is compacted to `ULV9Za34j0GZJ13FVSWOeA`.

### Formatter
Under the hood, the URL persistence service uses [`IUrlQueryStringFormatter`](xref:Tyne.Blazor.Persistence.IUrlQueryStringFormatter).
This can be used independently to generate links to pages without navigating.
