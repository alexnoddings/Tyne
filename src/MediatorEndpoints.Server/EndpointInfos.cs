using System.Reflection;

namespace Tyne.MediatorEndpoints;

public static class EndpointInfos
{
    private static readonly Type OpenGenericApiRequestType = typeof(IApiRequest<>);

    public static EndpointInfo[] GetFromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return GetCore(assembly).ToArray();
    }

    private static IEnumerable<EndpointInfo> GetCore(Assembly assembly)
    {
        var requestTypes = assembly.GetTypes().Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition);
        foreach (var requestType in requestTypes)
        {
            var responseTypes = requestType
                .GetInterfaces()
                .Where(interfaceType =>
                    interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == OpenGenericApiRequestType
                    && interfaceType.GenericTypeArguments.Length == 1
                )
                .Select(interfaceType => interfaceType.GenericTypeArguments[0])
                .ToArray();

            if (responseTypes.Length == 0)
                continue;

            if (responseTypes.Length > 1)
                throw new InvalidOperationException($"{requestType.Name} has multiple response types. It should only implement {nameof(IApiRequest)} once.");

            var metadata = requestType.GetCustomAttributes(true).AsReadOnly();
            yield return new EndpointInfo(requestType, responseTypes[0], metadata);
        }
    }
}
