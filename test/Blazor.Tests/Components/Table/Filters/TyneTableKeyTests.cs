using System.Reflection;

namespace Tyne.Blazor;

#pragma warning disable TYNE_OLD_TABLEKEY
public class TyneTableKeyTests
{
    [Fact]
    public void Default_IsValid()
    {
        // Ensures that default TyneTableKeys are still valid (i.e. Key is "", not null)
        var defaultKey = default(TyneTableKey);

        Assert.True(defaultKey.IsEmpty);
        Assert.Equal(string.Empty, defaultKey.Key);
    }

    public static object?[][] IsEmpty_Data { get; } =
        new object?[][]
        {
            new object?[] { default(TyneTableKey) },
            new object?[] { TyneTableKey.From(null) },
            new object?[] { TyneTableKey.From(string.Empty) },
            new object?[] { TyneTableKey.From("   ") },
            new object?[] { TyneTableKey.From(" \t\n\r ") },
            // If no property info is provided, the literal "*" should become ""
            new object?[] { TyneTableKey.From("*") }
        };

    [Theory]
    [MemberData(nameof(IsEmpty_Data))]
    public void IsEmpty(TyneTableKey key)
    {
        Assert.True(key.IsEmpty);
    }

    public static object?[][] IsNotEmpty_Data { get; } =
        new object?[][]
        {
            new object?[] { TyneTableKey.From("SomeKey") },
            new object?[] { TyneTableKey.From("  SomeKey  ") },
            new object?[] { TyneTableKey.From("\tA   ") }
        };

    [Theory]
    [MemberData(nameof(IsNotEmpty_Data))]
    public void IsNotEmpty(TyneTableKey key)
    {
        Assert.False(key.IsEmpty);
    }

    [Fact]
    public void Empty_IsEmpty()
    {
        // Assert
        Assert.True(TyneTableKey.Empty.IsEmpty);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    [InlineData(" \t ", " \t ")]
    [InlineData("K", "K")]
    [InlineData("SomeKey", "SomeKey")]
    [InlineData("\tSome_. Key ! ", "\tSome_. Key ! ")]
    [InlineData(null, "")]
    [InlineData(null, " \t ")]
    [InlineData("K", " \t K    ")]
    [InlineData("*", "")]
    public void Equals_AreEqual(string? inputString1, string? inputString2)
    {
        // Arrange
        var key1 = TyneTableKey.From(inputString1);
        var key2 = TyneTableKey.From(inputString2);

        // Assert
        Assert.Equal(key1, key2);
        Assert.True(key1 == key2);
        Assert.False(key1 != key2);
        Assert.True(key1.Equals(key2));
        Assert.True(key1.Equals(key2 as object));
        Assert.Equal(key1.GetHashCode(), key2.GetHashCode());
    }

    [Theory]
    [InlineData(null, "key")]
    [InlineData("", "key")]
    [InlineData(" \t ", "key")]
    [InlineData("K", "k")]
    [InlineData("Key1", "Key2")]
    [InlineData("A", "BCDEFG")]
    public void Equals_AreNotEqual(string? inputString1, string? inputString2)
    {
        // Arrange
        var key1 = TyneTableKey.From(inputString1);
        var key2 = TyneTableKey.From(inputString2);

        // Assert
        Assert.NotEqual(key1, key2);
        Assert.False(key1 == key2);
        Assert.True(key1 != key2);
        Assert.False(key1.Equals(key2));
        Assert.False(key1.Equals(key2 as object));
        Assert.NotEqual(key1.GetHashCode(), key2.GetHashCode());
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData(" ", "")]
    [InlineData(" \t ", "")]
    [InlineData("Key", "Key")]
    [InlineData(" Key\t ", "Key")]
    public void ImplicitToString_ReturnsKey(string? inputString, string expectedImplicitStringValue)
    {
        // Arrange
        var key = TyneTableKey.From(inputString);

        // Act
        string implicitStringValue = key;

        // Assert
        Assert.Equal(expectedImplicitStringValue, implicitStringValue);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData(" \t ", "")]
    [InlineData("SomeKey", "SomeKey")]
    [InlineData(" SomeKey ", "SomeKey")]
    [InlineData("\tSomeKey\t", "SomeKey")]
    [InlineData(" \t SomeKey \t ", "SomeKey")]
    [InlineData(" Some  Key ", "Some  Key")]
    [InlineData(". SomeKey .", ". SomeKey .")]
    public void From_HasCorrectKey(string? inputString, string expectedKey)
    {
        // Act
        // Both should act identically when PropertyInfo is null
        var key1 = TyneTableKey.From(inputString);
        var key2 = TyneTableKey.From(inputString, null);

        // Assert
        Assert.Equal(expectedKey, key1.Key);
        Assert.Equal(expectedKey, key2.Key);
    }

    public static PropertyInfo PropertyInfo1 { get; } =
        typeof(TyneTableKeyTests)
        .GetProperty(nameof(PropertyInfo1), BindingFlags.Public | BindingFlags.Static)!;

    public static object?[][] From_WithPropertyInfo_HasCorrectKey_Data { get; } =
        new object?[][]
        {
            new object?[] { null, PropertyInfo1, "" },
            new object?[] { "", PropertyInfo1, "" },
            new object?[] { "   ", PropertyInfo1, "" },
            new object?[] { "\t", PropertyInfo1, "" },
            new object?[] { "SomeKey", PropertyInfo1, "SomeKey" },
            new object?[] { "*SomeKey*", PropertyInfo1, "*SomeKey*" },
            new object?[] { "*K", PropertyInfo1, "*K" },
            new object?[] { "K*", PropertyInfo1, "K*" },
            new object?[] { "*K*", PropertyInfo1, "*K*" },
            new object?[] { "**", PropertyInfo1, "**" },
            new object?[] { " * ", PropertyInfo1, "*" },
            // Should only return the property's name if the input is the literal "*"
            new object?[] { "*", PropertyInfo1, PropertyInfo1.Name }
        };

    [Theory]
    [MemberData(nameof(From_WithPropertyInfo_HasCorrectKey_Data))]
    public void From_WithPropertyInfo_HasCorrectKey(string? inputString, PropertyInfo? propertyInfo, string expectedKey)
    {
        // Act
        var key = TyneTableKey.From(inputString, propertyInfo);

        // Assert
        Assert.Equal(expectedKey, key.Key);
    }
}
#pragma warning restore TYNE_OLD_TABLEKEY
