namespace Tyne.Results;

/// <summary>
///     <see cref="IMetadata"/> with an associated <see cref="Message"/>.
/// </summary>
public interface IMessageMetadata : IMetadata
{
	public string Message { get; }
}
