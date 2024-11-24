using System.Child;
using Tyne.EfTests;

namespace Tyne.EntityFramework;

public class DbContextChangeAuditorTypeNameFormatterTests
{
    public static TheoryData<string, Type> ExpectedTypeNames =>
        new()
        {
            // BCL types use their Type name not their keyword for simplicity
            { "Boolean", typeof(bool) },
            // Nullables should be shown as T? rather than Nullable<T>
            { "Guid?", typeof(Guid?) },
            // Types in the System namespace shouldn't have a namespace prefix
            { "String", typeof(string) },
            // Same as types in the System.Collections.Generic namespace
            { "List<Object>", typeof(List<object>) },
            // Only exact namespaces should be ignored, child namespaces shouldn't be ignored
            { "System.Child.SomeObject", typeof(SomeObject) },
            // Generics should be of the form T1<T2>, not T1[[T2]]
            { "KeyValuePair<Int32, String>", typeof(KeyValuePair<int, string>) },
            // Types outside of those namespaces should have a namespace prefix
            { "Tyne.EfTests.RootObject", typeof(RootObject) },
            // Nested types should retain the '+' nesting style for clarity
            { "Tyne.EfTests.RootObject+NestedEnum", typeof(RootObject.NestedEnum) },
            // Recursive generic
            { "List<HashSet<IEnumerable<Tyne.EfTests.RootEnum[]>>>", typeof(List<HashSet<IEnumerable<RootEnum[]>>>) },
            // We don't explicitly support open generic types (they don't have a use in change auditing concrete types),
            // but we should handle them gracefully rather than hitting an exception
            { "KeyValuePair<TKey, TValue>", typeof(KeyValuePair<,>) }
        };

    [Theory]
    [MemberData(nameof(ExpectedTypeNames))]
    public void FormatTypeName_ReturnsExpectedTypeName(string expectedTypeName, Type type)
    {
        // Act
        var actualTypeName = DbContextChangeAuditorTypeNameFormatter.FormatTypeName(type);

        // Assert
        Assert.Equal(expectedTypeName, actualTypeName);
    }
}
