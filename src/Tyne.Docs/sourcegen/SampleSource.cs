using System.Reflection;

namespace Tyne.Docs.SourceGen;

public static class SampleSource
{
	public static string? For(Type type) =>
		ForField(type, SampleSourceGenerator.SourceCodeFieldName);

	public static string? HtmlFor(Type type) =>
		ForField(type, SampleSourceGenerator.SourceCodeHtmlFieldName);

	private static string? ForField(Type type, string fieldName)
	{
		if (type is null)
			throw new ArgumentNullException(nameof(type));

		if (fieldName is null)
			throw new ArgumentNullException(nameof(fieldName));

		var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
		if (field is null)
			return null;

		return field.GetValue(null) as string;
	}
}
