using System.Resources;

namespace Tyne.HttpMediator.Server;

// These are pulled from ExceptionMessages.restext
internal static class ExceptionMessages
{
    private static readonly EmbeddedResourceManager _resources =
        EmbeddedResourceManager.GetFor(typeof(ExceptionMessages));

    internal static string HandleRequest_NoQueryParameter => _resources.GetMemberString(culture: default, arg0: HttpMediatorConventions.UriQueryStringParameterName);

    internal static readonly string HttpResponse_HasStarted_CannotWrite = _resources.GetMemberString(culture: default);
    internal static readonly string HttpResponse_HasStarted_EnsureMiddlewareTerminates = _resources.GetMemberString(culture: default);

    internal static string JsonConversionForTypeNotSupported(Type type) => _resources.GetMemberString(culture: default, arg0: type.Name);
    internal static readonly string JsonConverterFactoryCouldNotCreateConverter = _resources.GetMemberString(culture: default);

    internal static string MediatorExecutorNotRegistered(Type type) => _resources.GetMemberString(culture: default, arg0: type.Name);
    internal static string MediatorExecutorInvalidImplementation(Type type) => _resources.GetMemberString(culture: default, arg0: type.Name);

}
