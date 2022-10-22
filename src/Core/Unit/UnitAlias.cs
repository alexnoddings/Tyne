using System.Diagnostics.CodeAnalysis;

namespace Tyne;

/// <summary>
///		To allow the use of <c>unit</c> in your code, add the following to your <c>.csproj</c> or <c>Directory.Build.props</c>:
///		<code>
///			&lt;ItemGroup&gt;
///				&lt;Using Include="Tyne.UnitAlias" Static="true" /&gt;
///			&lt;/ItemGroup&gt;
///		</code>
/// </summary>
public static class UnitAlias
{
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Intentionally designed to allow `unit` to be used.")]
	public static ref readonly Unit unit => ref Unit.Value;
}
