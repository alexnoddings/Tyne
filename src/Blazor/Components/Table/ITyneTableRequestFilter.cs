namespace Tyne.Blazor;

public interface ITyneTableRequestFilter<in TRequest>
{
    public void ConfigureRequest(TRequest request);
}
