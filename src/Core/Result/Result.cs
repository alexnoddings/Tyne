using MediatR;

namespace Tyne;

public static class Result
{
    public static Result<Unit> Success() =>
        Success(Unit.Value);

    public static Result<Unit> Success(params string[] messages) =>
        Success(Unit.Value, messages);

    public static Result<Unit> Success(params ResultMessage[] messages) =>
        Success(Unit.Value, messages);

    public static Result<Unit> Success(IEnumerable<ResultMessage> messages) =>
        Success(Unit.Value, messages);

    public static Result<T> Success<T>(T value) =>
        Success(value, Array.Empty<ResultMessage>() as IList<ResultMessage>);

    public static Result<T> Success<T>(T value, params string[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        var messagesList = messages
            .Select(message => new ResultMessage(ResultMessageType.Success, message))
            .ToList();
        return Success(value, messagesList);
    }

    public static Result<T> Success<T>(T value, params ResultMessage[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        return Success(value, messages as IList<ResultMessage>);
    }

    public static Result<T> Success<T>(T value, IEnumerable<ResultMessage> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        return Result<T>.Success(value, messages.ToList());
    }

    public static Result<Unit> Failure(string message, params string[] messages)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(messages);

        var messagesList = messages
            .Prepend(message)
            .Select(message => new ResultMessage(ResultMessageType.Error, message))
            .ToList();
        return Failure(messagesList);
    }

    public static Result<Unit> Failure(ResultMessage message, params ResultMessage[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        return Failure(messages.Prepend(message));
    }

    public static Result<Unit> Failure(IEnumerable<ResultMessage> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        return Result<Unit>.Failure(messages.ToList());
    }

    public static Result<T> Failure<T>(string message, params string[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        var messagesList = messages
            .Prepend(message)
            .Select(message => new ResultMessage(ResultMessageType.Error, message))
            .ToList();
        return Failure<T>(messagesList);
    }

    public static Result<T> Failure<T>(ResultMessage message, params ResultMessage[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        return Failure<T>(messages.Prepend(message));
    }

    public static Result<T> Failure<T>(IEnumerable<ResultMessage> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);
        return Result<T>.Failure(messages.ToList());
    }

    internal static bool WasSuccess(IEnumerable<ResultMessage> messages) =>
        !messages.Any(message => message.Type is ResultMessageType.Error);
}