using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Tyne;

public class ExpressionHelperTests
{
    [Fact]
    public void Get_PropertyInfo_Field_NotFound()
    {
        Expression<Func<TestClass, int>> expression = test => test.Field;

        var propertyInfo = ExpressionHelper.TryGetAccessedPropertyInfo(expression);
        Assert.Null(propertyInfo);

        _ = Assert.Throws<ArgumentException>(() => ExpressionHelper.GetAccessedPropertyInfo(expression));
    }

    [Fact]
    public void Get_PropertyInfo_Property_Found()
    {
        Expression<Func<TestClass, int>> expression = test => test.Property;
        var propertyInfo = ExpressionHelper.TryGetAccessedPropertyInfo(expression);
        Assert.NotNull(propertyInfo);

        var manualPropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.Property));
        Assert.Equal(manualPropertyInfo, propertyInfo);
    }

    [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Done for testing.")]
    [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Done for testing.")]
    public class TestClass
    {
        public int Field = 42;
        public int Property { get; set; } = 42;
    }
}
