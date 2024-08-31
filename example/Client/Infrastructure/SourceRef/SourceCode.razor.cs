using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor.Utilities;
using Tyne.Aerospace.Client.Infrastructure.SourceRef;

namespace Tyne.Aerospace.Client.Infrastructure;

public partial class SourceCode<T>
{
    private readonly string _id = Guid.NewGuid().ToString("N");

    [Parameter, EditorRequired]
    public SourceCodeType SourceType { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Name { get; set; } = null!;

    [Parameter]
    public string? ContentClass { get; set; }

    [Parameter]
    public bool ShowSourceByDefault { get; set; } = true;

    private bool ShowSource { get; set; }

    private string SourceCodeString { get; set; } = string.Empty;
    private string SourceCodePath { get; set; } = string.Empty;

    [Inject]
    private IJSRuntime Js { get; init; } = null!;

    private string CodeClassName =>
        new CssBuilder("hljs rounded-b")
        .AddClass("lang-razor language-cshtml-razor", SourceType is SourceCodeType.Component)
        .AddClass("lang-csharp language-csharp", SourceType is SourceCodeType.Type)
        .Build();

    private string ContentClassName =>
        ContentClass ?? "pa-4";

    protected override void OnInitialized()
    {
        ShowSource = ShowSourceByDefault;

        SourceCodeString = SourceLookup.Source<T>(SourceType);
        SourceCodePath = SourceLookup.Path<T>(SourceType);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await Js.InvokeVoidAsync("highlightCodeBlock", _id);
    }

    private string DefaultName =>
       typeof(T).Name + GetSourceTypeExtension();

    private string GetSourceTypeExtension() =>
        SourceType switch
        {
            SourceCodeType.Component => ".razor",
            SourceCodeType.Type => ".cs",
            _ => ""
        };
}
