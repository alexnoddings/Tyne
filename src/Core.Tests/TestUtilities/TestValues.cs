using Tyne.TestUtilities;

namespace Tyne;

internal static class TestValues
{
	internal const int Ok = 42;
	internal const string ErrorMessage = "Some error message.";

	internal static readonly TestErrorKind ErrorKind = TestErrorKind.ExampleError2;

	internal static readonly TestType Test = new(Ok);
	internal static readonly TestInheritedType TestInherited = new(Ok);

	internal static readonly HumanError Error = new(ErrorMessage);
	internal static readonly TestHumanError TestError = new(ErrorMessage);
	internal static readonly TestInheritedError TestInheritedError = new(ErrorMessage);
}
