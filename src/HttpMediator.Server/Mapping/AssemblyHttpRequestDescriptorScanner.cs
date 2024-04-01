using System.Reflection;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     Helper class for generating <see cref="HttpRequestDescriptor"/>s from an <see cref="Assembly"/>.
/// </summary>
internal static class AssemblyHttpRequestDescriptorScanner
{
    private static readonly Type OpenGenericHttpRequestType = typeof(IHttpRequestBase<>);

    /// <summary>
    ///     Gets <see cref="HttpRequestDescriptor"/>s for HTTP requests defined in <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to scan for <see cref="IHttpRequestBase{TResponse}"/>s.</param>
    /// <returns>An array of <see cref="HttpRequestDescriptor"/>s.</returns>
    public static HttpRequestDescriptor[] GetDescriptorsFromAssembly(Assembly assembly) =>
        HttpRequestDescriptorIterator(assembly).ToArray();

    private static IEnumerable<HttpRequestDescriptor> HttpRequestDescriptorIterator(Assembly assembly)
    {
        // Find non-abstract, non-generic types
        var requestTypes = assembly.GetTypes().Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition);
        foreach (var requestType in requestTypes)
        {
            // Check if the type implements IHttpRequest<TRequest, TResponse>, and if so find the TRequest type
            var responseTypes = requestType
                .GetInterfaces()
                .Where(interfaceType =>
                    interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == OpenGenericHttpRequestType
                    && interfaceType.GenericTypeArguments.Length == 1
                )
                .Select(interfaceType => interfaceType.GenericTypeArguments[0])
                .ToArray();

            // The type isn't an IHttpRequest<,>
            if (responseTypes.Length == 0)
                continue;

            // The type implements IHttpRequest<,> more than once
            if (responseTypes.Length > 1)
                throw new InvalidOperationException($"{requestType.Name} has multiple response types. It should only implement {nameof(IHttpRequestBase)} once.");

            // Pull all of the metadata for the type
            var metadata = requestType.GetCustomAttributes(inherit: true).AsReadOnly();
            yield return new HttpRequestDescriptor(requestType, responseTypes[0], metadata);
        }
    }
}
