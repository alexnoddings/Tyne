namespace Tyne;

public class UnitTests
{
	// Unit.ctor() has a diagnostic to prevent users from instantiating it.
	// This causes a build error if we directly create one, so instead we
	// create it through a generic method.
	private static T Create<T>() where T : new() => new();

	public static object[][] Units() => new[]
	{
		new object[] { Unit.Value },
		new object[] { Create<Unit>() },
		new object[] { default(Unit) }
	};

	[Theory]
	[MemberData(nameof(Units))]
	public void Units_ShouldBeEqual(Unit unit2)
	{
		// Arrange
		var unit1 = Unit.Value;

		// Assert
		Assert.Equal(unit1, unit2);
		Assert.True(unit1 == unit2);
		Assert.False(unit1 != unit2);
		Assert.True(unit1.Equals(unit2));
		Assert.True(unit1.Equals(unit2 as object));
	}

	public static object?[][] NotEqualObjects() => new[]
	{
		new object?[] { null },
		new object[] { -1 },
		new object[] { new object() },
		new object[] { Array.Empty<object>() },
		new object[] { string.Empty },
		new object[] { Unit.Value.ToString() },
		new object[] { Unit.Value.AsTask },
		new object[] { Unit.Value.AsValueTask },
	};

	[Theory]
	[MemberData(nameof(NotEqualObjects))]
	public void NotUnits_ShouldNotBeEqual(object value)
	{
		// Arrange
		var unit1 = Unit.Value;

		// Assert.NotEqual(unit, value) uses CompareTo, which will always return 0
		Assert.False(unit1.Equals(value));
	}

	[Fact]
	public async Task Tasks_ShouldBeEqual()
	{
		// Arrange
		var unit1 = Unit.Value;

		// Assert
		Assert.Equal(unit1, await Unit.Value.AsTask);
		Assert.Equal(unit1, await Unit.Value.AsValueTask);
	}

	[Fact]
	public void Units_ShouldBeComparable()
	{
		// Arrange
		var unit1 = Unit.Value;
		var unit2 = Unit.Value;

		// Assert
		Assert.False(unit1 < unit2);
		Assert.True(unit1 <= unit2);

		Assert.False(unit1 > unit2);
		Assert.True(unit1 >= unit2);

		Assert.Equal(0, unit1.CompareTo(unit2));
		Assert.Equal(0, unit1.CompareTo(unit2 as object));

		Assert.Equal(0, unit1.CompareTo(string.Empty));
		Assert.Equal(0, unit1.CompareTo(null));
	}

	[Fact]
	public void ToString_ReturnsEmptyBrackets()
	{
		// Arrange
		var unit1 = Unit.Value;

		// Assert
		Assert.Equal("()", unit1.ToString());
	}

	[Fact]
	public void GetHashCode_Returns0()
	{
		// Arrange
		var unit1 = Unit.Value;

		// Assert
		Assert.Equal(0, unit1.GetHashCode());
	}

	[Fact]
	public void Alias_Works()
	{
		Assert.IsType<Unit>(UnitAlias.unit);
		Assert.IsType<Unit>(unit);
	}
}
