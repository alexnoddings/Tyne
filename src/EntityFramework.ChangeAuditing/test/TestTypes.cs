// These are dummy types, these rules don't make sense
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
// Namespace intentionally shortened to make type names easier to read for tests
namespace Tyne.EfTests
{

    internal sealed class RootObject
    {
        internal sealed class NestedObject;
        internal enum NestedEnum;
    }

    internal enum RootEnum;
}

namespace System.Child
{
    internal sealed class SomeObject;
}
#pragma warning restore CA1812
#pragma warning restore S1144
#pragma warning restore S2094
#pragma warning restore S2344
