namespace Tyne.Core;

public class UnitTests
{
	[Fact]
	public void ShouldBeEqual()
	{
		var unit1 = Unit.Value;
		var unit2 = Unit.Value;

		Assert.Equal(unit1, unit2);
		Assert.True(unit1 == unit2);
		Assert.True(unit1.Equals(unit2));
		Assert.True(unit1.Equals(unit2 as object));
		Assert.False(unit1 != unit2);
	}

	public static object[] NotEqualObjects() => new[]
	{
		new object[] { string.Empty },
		new object[] { -1 },
		new object[] { Unit.AsTask },
		new object[] { new object() },
		new object[] { Array.Empty<object>() }
	};

	[Theory]
	[MemberData(nameof(NotEqualObjects))]
	public void ShouldNotBeEqual(object value)
	{
		var unit1 = Unit.Value;

		// Assert.NotEqual(unit, value) uses CompareTo, which will always return 0
		Assert.False(unit1.Equals(value));
	}

	[Fact]
	public void ShouldNotBeEqualToNull()
	{
		var unit1 = Unit.Value;
		Assert.False(unit1.Equals(null));
	}

	[Fact]
	public async Task ReferencesAreEqual()
	{
		var unit1 = Unit.Value;

		Assert.Equal(default, unit1);
		Assert.Equal(unit1, await Unit.AsTask);
		Assert.Equal(unit1, await Unit.AsValueTask);
		// Ignore "'Unit.Unit()' is obsolete: 'Use Value instead.'"
#pragma warning disable TYN001
		Assert.Equal(unit1, new Unit());
#pragma warning restore TYN001
	}

	[Fact]
	public void ShouldBeComparable()
	{
		var unit1 = Unit.Value;
		var unit2 = Unit.Value;

		Assert.Equal(0, unit1.CompareTo(unit2));
		Assert.Equal(0, unit1.CompareTo(unit2 as object));

		Assert.False(unit1 < unit2);
		Assert.True(unit1 <= unit2);
		Assert.False(unit1 > unit2);
		Assert.True(unit1 >= unit2);
	}

	[Fact]
	public void ToStringWorks()
	{
		var unit1 = Unit.Value;

		Assert.Equal("()", unit1.ToString());
	}
}
