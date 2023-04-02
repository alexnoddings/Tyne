namespace Tyne;

public record ResultMessage
{
    public ResultMessageType Type { get; }
    public string Message { get; }

    public ResultMessage(ResultMessageType type, string message)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentOutOfRangeException(nameof(type), type, $"Undefined {nameof(ResultMessageType)}.");

        ArgumentException.ThrowIfNullOrEmpty(message);

        Type = type;
        Message = message;
    }

    public static implicit operator ResultMessage((ResultMessageType, string) tuple) =>
        FromValueTuple(tuple);

    public static ResultMessage FromValueTuple((ResultMessageType Type, string Message) tuple) =>
        new(tuple.Type, tuple.Message);
}
