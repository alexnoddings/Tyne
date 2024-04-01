using System.Resources;

namespace Tyne.HttpMediator.Server;

// These are pulled from ExceptionMessages.restext
internal static class ExceptionMessages
{
    private static readonly EmbeddedResourceManager Resources =
        EmbeddedResourceManager.GetFor(typeof(ExceptionMessages));

    internal static string HandleRequest_NoQueryParameter => Resources.GetMemberString(culture: default, arg0: HttpMediatorConventions.UriQueryStringParameterName);

    internal static readonly string HttpResponse_HasStarted_CannotWrite = Resources.GetMemberString(culture: default);
    internal static readonly string HttpResponse_HasStarted_EnsureMiddlewareTerminates = Resources.GetMemberString(culture: default);

    internal static string JsonConversionForTypeNotSupported(Type type) => Resources.GetMemberString(culture: default, arg0: type.Name);
    internal static readonly string JsonConverterFactoryCouldNotCreateConverter = Resources.GetMemberString(culture: default);

    internal static string MediatorExecutorNotRegistered(Type type) => Resources.GetMemberString(culture: default, arg0: type.Name);
    internal static string MediatorExecutorInvalidImplementation(Type type) => Resources.GetMemberString(culture: default, arg0: type.Name);

}
