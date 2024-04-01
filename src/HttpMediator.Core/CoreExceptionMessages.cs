using System.Resources;

namespace Tyne.HttpMediator;

// These are pulled from CoreExceptionMessages.restext
internal static class CoreExceptionMessages
{
    private static readonly EmbeddedResourceManager Resources =
        EmbeddedResourceManager.GetFor(typeof(CoreExceptionMessages));

    internal static readonly string HttpResult_ErrorStatusCodeOutOfRange = Resources.GetMemberString(culture: default);
    internal static readonly string HttpResult_OkStatusCodeOutOfRange = Resources.GetMemberString(culture: default);
    internal static readonly string HttpResult_OkMustHaveValue = Resources.GetMemberString(culture: default);

    internal static string ApiRequest_HttpMethodNotSupported(Type type, string methodName) =>
        Resources.GetMemberString(culture: default, arg0: methodName, arg1: type.Name);
}
