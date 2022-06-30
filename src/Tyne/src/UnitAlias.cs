using System.Diagnostics.CodeAnalysis;

namespace Tyne;

public static class UnitAlias
{
	[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Intentionally designed to allow `unit` as a type when statically used.")]
	public static ref readonly Unit unit => ref Unit.Value;
}
