using System.Resources;

namespace Tyne;

// These are pulled from ExceptionMessages.restext
internal static class ExceptionMessages
{
    private static readonly EmbeddedResourceManager _resources =
        EmbeddedResourceManager.GetFor(typeof(ExceptionMessages));

    internal static readonly string ExceptionFactoryReturnedNull = _resources.GetMemberString(culture: default);

    internal static string JsonConversionForTypeNotSupported(Type type) => _resources.GetMemberString(culture: default, arg0: type.Name);
    internal static readonly string JsonConverterFactoryCouldNotCreateConverter = _resources.GetMemberString(culture: default);

    internal static readonly string Option_CannotUnwrapNone = _resources.GetMemberString(culture: default);
    internal static readonly string Option_Invalid = _resources.GetMemberString(culture: default);
    internal static readonly string Option_NoneHasNoValue = _resources.GetMemberString(culture: default);
    internal static readonly string Option_SomeMustHaveValue = _resources.GetMemberString(culture: default);

    internal static readonly string Result_CannotUnwrapErrorFromOk = _resources.GetMemberString(culture: default);
    internal static readonly string Result_CannotUnwrapValueFromError = _resources.GetMemberString(culture: default);
    internal static readonly string Result_Invalid = _resources.GetMemberString(culture: default);
    internal static readonly string Result_ErrorHasNoValue = _resources.GetMemberString(culture: default);
    internal static readonly string Result_ErrorMustHaveError = _resources.GetMemberString(culture: default);
    internal static readonly string Result_OkHasNoError = _resources.GetMemberString(culture: default);
    internal static readonly string Result_OkMustHaveValue = _resources.GetMemberString(culture: default);
}
