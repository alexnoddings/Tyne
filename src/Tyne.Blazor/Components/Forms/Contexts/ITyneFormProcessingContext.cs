namespace Tyne.Blazor;

public interface ITyneFormProcessingContext
{
    public bool IsProcessing { get; }

    public void Deconstruct(out bool isProcessing)
    {
        isProcessing = IsProcessing;
    }
}
