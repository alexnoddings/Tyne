using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     Extensions for working with <see cref="HttpResponse"/>s.
/// </summary>
internal static class HttpResponseExtensions
{
    /// <summary>
    ///     Ensures the <paramref name="httpResponse"/> has not started yet.
    ///     If it has, throws an <see cref="InvalidOperationException"/> with <paramref name="message"/>.
    /// </summary>
    /// <param name="httpResponse">The <see cref="HttpResponse"/>.</param>
    /// <param name="message">The exception message to throw if <paramref name="httpResponse"/> has started.</param>
    /// <exception cref="InvalidOperationException">If <paramref name="httpResponse"/> has started.</exception>
    [StackTraceHidden]
    public static void EnsureHasNotStarted(this HttpResponse httpResponse, string message)
    {
        if (httpResponse.HasStarted)
            throw new InvalidOperationException(message);
    }

    /// <summary>
    ///     Ensures the <paramref name="httpResponse"/> has not started yet.
    ///     If it has, throws an <see cref="InvalidOperationException"/> with a default message.
    /// </summary>
    /// <param name="httpResponse">The <see cref="HttpResponse"/>.</param>
    /// <exception cref="InvalidOperationException">If <paramref name="httpResponse"/> has started.</exception>
    [StackTraceHidden]
    public static void EnsureHasNotStarted(this HttpResponse httpResponse) =>
        EnsureHasNotStarted(httpResponse, ExceptionMessages.HttpResponse_HasStarted_CannotWrite);
}
