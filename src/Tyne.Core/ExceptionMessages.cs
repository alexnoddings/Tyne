using System.Resources;

namespace Tyne;

// These are pulled from ExceptionMessages.restext
internal static class ExceptionMessages
{
    private static readonly EmbeddedResourceManager _resources =
        EmbeddedResourceManager.GetFor(typeof(ExceptionMessages));

    internal static string JsonConverter_ConversionForTypeNotSupported(Type type) => _resources.GetMemberString(culture: null, arg0: type.Name);
    internal static readonly string JsonConverter_FactoryCouldNotCreateConverter = _resources.GetMemberString(culture: null);

    internal static string Result_JsonConverter_InvalidType(string? type) => _resources.GetMemberString(culture: null, arg0: type);
    internal static readonly string Result_JsonConverter_NoResultType = _resources.GetMemberString(culture: null);
    internal static readonly string Result_JsonConverter_OkButNoValue = _resources.GetMemberString(culture: null);
    internal static readonly string Result_JsonConverter_ErrorButNoError = _resources.GetMemberString(culture: null);
}
