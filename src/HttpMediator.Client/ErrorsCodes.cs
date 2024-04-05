namespace Tyne.HttpMediator.Client;

/// <summary>
///     Errors codes for <see cref="Error"/>s created as part of <c>Tyne.HttpMediator.Client</c>.
/// </summary>
public static class ErrorsCodes
{
    /// <summary>
    ///     The error code used when an unhandled exception is caught in the middleware pipeline.
    /// </summary>
    public static readonly string UnhandledExceptionMiddleware = "Tyne.HttpMediator.Client.UnhandledException";
}
