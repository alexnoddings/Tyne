using System.Collections.ObjectModel;

namespace Tyne;

public interface IResult
{
    public bool WasSuccess { get; }
    public ReadOnlyCollection<ResultMessage> Messages { get; }
}