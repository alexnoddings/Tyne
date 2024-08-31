using System.Resources;

namespace Tyne.HttpMediator;

// These are pulled from CoreExceptionMessages.restext
internal static class CoreExceptionMessages
{
    private static readonly EmbeddedResourceManager _resources =
        EmbeddedResourceManager.GetFor(typeof(CoreExceptionMessages));

    internal static readonly string HttpResult_ErrorStatusCodeOutOfRange = _resources.GetMemberString(culture: default);
    internal static readonly string HttpResult_OkStatusCodeOutOfRange = _resources.GetMemberString(culture: default);
    internal static readonly string HttpResult_OkMustHaveValue = _resources.GetMemberString(culture: default);

    internal static string ApiRequest_HttpMethodNotSupported(Type type, string methodName) =>
        _resources.GetMemberString(culture: default, arg0: methodName, arg1: type.Name);
}
