using System.Resources;

namespace Tyne;

// These are pulled from ExceptionMessages.restext
internal static class ExceptionMessages
{
    private static readonly EmbeddedResourceManager Resources =
        EmbeddedResourceManager.GetFor(typeof(ExceptionMessages));

    internal static readonly string ExceptionFactoryReturnedNull = Resources.GetMemberString(culture: default);

    internal static string JsonConversionForTypeNotSupported(Type type) => Resources.GetMemberString(culture: default, arg0: type.Name);
    internal static readonly string JsonConverterFactoryCouldNotCreateConverter = Resources.GetMemberString(culture: default);

    internal static readonly string Option_CannotUnwrapNone = Resources.GetMemberString(culture: default);
    internal static readonly string Option_Invalid = Resources.GetMemberString(culture: default);
    internal static readonly string Option_NoneHasNoValue = Resources.GetMemberString(culture: default);
    internal static readonly string Option_SomeMustHaveValue = Resources.GetMemberString(culture: default);

    internal static readonly string Result_CannotUnwrapErrorFromOk = Resources.GetMemberString(culture: default);
    internal static readonly string Result_CannotUnwrapValueFromError = Resources.GetMemberString(culture: default);
    internal static readonly string Result_Invalid = Resources.GetMemberString(culture: default);
    internal static readonly string Result_ErrorHasNoValue = Resources.GetMemberString(culture: default);
    internal static readonly string Result_ErrorMustHaveError = Resources.GetMemberString(culture: default);
    internal static readonly string Result_OkHasNoError = Resources.GetMemberString(culture: default);
    internal static readonly string Result_OkMustHaveValue = Resources.GetMemberString(culture: default);
}
