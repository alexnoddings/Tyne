using System.Diagnostics;
using System.Reflection;

namespace Tyne.Utilities;

/// <summary>
///     Extensions for workign with <see cref="MethodInfo"/>s.
/// </summary>
public static class MethodInfoExtensions
{
    /// <summary>
    ///     Invokes <paramref name="methodInfo"/> with <paramref name="obj"/> and <paramref name="parameters"/>.
    ///     The returned <see cref="object"/> is cast to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type returned by <paramref name="methodInfo"/>.</typeparam>
    /// <param name="methodInfo">The <see cref="MethodInfo"/> instance.</param>
    /// <param name="obj">
    ///     The <see cref="object"/> on which to invoke the method.
    ///     If a method is <see langword="static"/>, this argument is ignored.
    /// </param>
    /// <param name="parameters">
    ///     <para>
    ///         An argument list for the invoked method.
    ///         This is an array of <see cref="object"/>s with the same number, order, and <see cref="Type"/> as the parameters of the method to be invoked.
    ///         If there are no parameters, <paramref name="parameters"/> should be <see langword="null"/>.
    ///     </para>
    ///     <para>
    ///         If the method represented by <paramref name="methodInfo"/> takes a <see langword="ref"/> parameter,
    ///         no special attribute is required for that parameter in order to invoke the method using this function.
    ///         Any object in this array that is not explicitly initialized with a value will contain the <see langword="default"/> value for that object type.
    ///     </para>
    /// </param>
    /// <returns>
    ///     The <c><typeparamref name="TResult"/>?</c> returned when invoking <paramref name="methodInfo"/>.
    /// </returns>
    /// <remarks>
    ///     This will catch <see cref="TargetInvocationException"/>s, and rethrow the base <see cref="Exception"/>.
    /// </remarks>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static TResult? Invoke<TResult>(this MethodInfo methodInfo, object? obj, params object?[]? parameters)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);
        try
        {
            return (TResult?)methodInfo.Invoke(obj, parameters);
        }
        catch (TargetInvocationException invocationException)
        {
            // Reflection wraps exceptions in a TargetInvocationException,
            // unwrap the base exception to expose for the stack trace
            throw invocationException.GetBaseException();
        }
    }

    [StackTraceHidden]
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static TResult? Invoke<TResult>(this MethodInfo methodInfo, object? obj) =>
        Invoke<TResult>(methodInfo, obj, parameters: null);

    /// <summary>
    ///     Invokes <paramref name="methodInfo"/> with <paramref name="obj"/> and <paramref name="parameters"/>.
    ///     <paramref name="methodInfo"/> is expected to return a <see cref="Task"/>
    ///     The returned <see cref="object"/> is cast to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type returned by <paramref name="methodInfo"/>.</typeparam>
    /// <param name="methodInfo">The <see cref="MethodInfo"/> instance.</param>
    /// <param name="obj">
    ///     The <see cref="object"/> on which to invoke the method.
    ///     If a method is <see langword="static"/>, this argument is ignored.
    /// </param>
    /// <param name="parameters">
    ///     <para>
    ///         An argument list for the invoked method.
    ///         This is an array of <see cref="object"/>s with the same number, order, and <see cref="Type"/> as the parameters of the method to be invoked.
    ///         If there are no parameters, <paramref name="parameters"/> should be <see langword="null"/>.
    ///     </para>
    ///     <para>
    ///         If the method represented by <paramref name="methodInfo"/> takes a <see langword="ref"/> parameter,
    ///         no special attribute is required for that parameter in order to invoke the method using this function.
    ///         Any object in this array that is not explicitly initialized with a value will contain the <see langword="default"/> value for that object type.
    ///     </para>
    /// </param>
    /// <returns>
    ///     The <c><typeparamref name="TResult"/>?</c> returned when the invocation <paramref name="methodInfo"/> has been awaited.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This will catch <see cref="TargetInvocationException"/>s thrown by <paramref name="methodInfo"/> (during sync or async execution), and rethrow the base <see cref="Exception"/>.
    ///     </para>
    ///     <para>
    ///         This ONLY supports <paramref name="methodInfo"/>s which return a <see cref="Task{TResult}"/>.
    ///         Since C# implements <see langword="await"/>ing through duck typing, we can't easily reflect awaiting on other types.
    ///     </para>
    /// </remarks>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static async Task<TResult?> InvokeAsync<TResult>(this MethodInfo methodInfo, object? obj, params object?[]? parameters)
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        try
        {
            return methodInfo.Invoke(obj, parameters) is Task<TResult> returnedTask
                ? await returnedTask.ConfigureAwait(false)
                : default;
        }
        catch (TargetInvocationException invocationException)
        {
            // Reflection wraps exceptions in a TargetInvocationException,
            // unwrap the base exception to expose for the stack trace
            throw invocationException.GetBaseException();
        }
    }

    /// <inheritdoc cref="InvokeAsync{TResult}(MethodInfo, object, object[])"/>
    [StackTraceHidden]
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static Task<TResult?> InvokeAsync<TResult>(this MethodInfo methodInfo, object? obj) =>
        InvokeAsync<TResult>(methodInfo, obj, parameters: null);
}
