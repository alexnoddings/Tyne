using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tyne;

public class MethodHelperTests
{
    private static TestClass TestInstance { get; } = new();

    private const BindingFlags InternalBindingFlags = MethodHelper.DefaultBindingFlags | BindingFlags.NonPublic;

    [Fact]
    public void Get_Instance_T0_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass>(nameof(TestClass.PublicInstanceMethod));
        var result = methodInfo.Invoke(TestInstance, null);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Get_Instance_T0_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass>(nameof(TestClass.InternalInstanceMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(TestInstance, null);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Get_Instance_T0_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass>(nameof(TestClass.InternalInstanceMethod)));
    }

    [Fact]
    public void Get_Instance_T1_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int>(nameof(TestClass.PublicInstanceMethod));
        var result = methodInfo.Invoke(TestInstance, new object[] { 1 });
        Assert.Equal(1, result);
    }

    [Fact]
    public void Get_Instance_T1_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int>(nameof(TestClass.InternalInstanceMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(TestInstance, new object[] { 1 });
        Assert.Equal(1, result);
    }

    [Fact]
    public void Get_Instance_T1_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int>(nameof(TestClass.InternalInstanceMethod)));
    }

    [Fact]
    public void Get_Instance_T2_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int>(nameof(TestClass.PublicInstanceMethod));
        var result = methodInfo.Invoke(TestInstance, new object[] { 1, 2 });
        Assert.Equal(3, result);
    }

    [Fact]
    public void Get_Instance_T2_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int>(nameof(TestClass.InternalInstanceMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(TestInstance, new object[] { 1, 2 });
        Assert.Equal(3, result);
    }

    [Fact]
    public void Get_Instance_T2_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int, int>(nameof(TestClass.InternalInstanceMethod)));
    }

    [Fact]
    public void Get_Instance_T3_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int>(nameof(TestClass.PublicInstanceMethod));
        var result = methodInfo.Invoke(TestInstance, new object[] { 1, 2, 3 });
        Assert.Equal(6, result);
    }

    [Fact]
    public void Get_Instance_T3_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int>(nameof(TestClass.InternalInstanceMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(TestInstance, new object[] { 1, 2, 3 });
        Assert.Equal(6, result);
    }

    [Fact]
    public void Get_Instance_T3_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int, int, int>(nameof(TestClass.InternalInstanceMethod)));
    }

    [Fact]
    public void Get_Instance_T4_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int, int>(nameof(TestClass.PublicInstanceMethod));
        var result = methodInfo.Invoke(TestInstance, new object[] { 1, 2, 3, 4 });
        Assert.Equal(10, result);
    }

    [Fact]
    public void Get_Instance_T4_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int, int>(nameof(TestClass.InternalInstanceMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(TestInstance, new object[] { 1, 2, 3, 4 });
        Assert.Equal(10, result);
    }

    [Fact]
    public void Get_Instance_T4_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int, int, int, int>(nameof(TestClass.InternalInstanceMethod)));
    }


    [Fact]
    public void Get_Static_T0_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass>(nameof(TestClass.PublicStaticMethod));
        var result = methodInfo.Invoke(null, null);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Get_Static_T0_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass>(nameof(TestClass.InternalStaticMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(null, null);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Get_Static_T0_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass>(nameof(TestClass.InternalStaticMethod)));
    }

    [Fact]
    public void Get_Static_T1_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int>(nameof(TestClass.PublicStaticMethod));
        var result = methodInfo.Invoke(null, new object[] { 1 });
        Assert.Equal(1, result);
    }

    [Fact]
    public void Get_Static_T1_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int>(nameof(TestClass.InternalStaticMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(null, new object[] { 1 });
        Assert.Equal(1, result);
    }

    [Fact]
    public void Get_Static_T1_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int>(nameof(TestClass.InternalStaticMethod)));
    }

    [Fact]
    public void Get_Static_T2_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int>(nameof(TestClass.PublicStaticMethod));
        var result = methodInfo.Invoke(null, new object[] { 1, 2 });
        Assert.Equal(3, result);
    }

    [Fact]
    public void Get_Static_T2_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int>(nameof(TestClass.InternalStaticMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(null, new object[] { 1, 2 });
        Assert.Equal(3, result);
    }

    [Fact]
    public void Get_Static_T2_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int, int>(nameof(TestClass.InternalStaticMethod)));
    }

    [Fact]
    public void Get_Static_T3_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int>(nameof(TestClass.PublicStaticMethod));
        var result = methodInfo.Invoke(null, new object[] { 1, 2, 3 });
        Assert.Equal(6, result);
    }

    [Fact]
    public void Get_Static_T3_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int>(nameof(TestClass.InternalStaticMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(null, new object[] { 1, 2, 3 });
        Assert.Equal(6, result);
    }

    [Fact]
    public void Get_Static_T3_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int, int, int>(nameof(TestClass.InternalStaticMethod)));
    }

    [Fact]
    public void Get_Static_T4_Public()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int, int>(nameof(TestClass.PublicStaticMethod));
        var result = methodInfo.Invoke(null, new object[] { 1, 2, 3, 4 });
        Assert.Equal(10, result);
    }

    [Fact]
    public void Get_Static_T4_Internal_Found()
    {
        var methodInfo = MethodHelper.Get<TestClass, int, int, int, int>(nameof(TestClass.InternalStaticMethod), InternalBindingFlags);
        var result = methodInfo.Invoke(null, new object[] { 1, 2, 3, 4 });
        Assert.Equal(10, result);
    }

    [Fact]
    public void Get_Static_T4_Internal_NotFound()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass, int, int, int, int>(nameof(TestClass.InternalStaticMethod)));
    }

    [Fact]
    public void Get_NonExistent_Throws()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass>("NonExistentMethod"));
    }

    [Fact]
    public void Get_Overload_Throws()
    {
        Assert_ThrowsArgumentException(() => MethodHelper.Get<TestClass>(nameof(TestClass.PublicInstanceOverloaded)));
    }

    private static void Assert_ThrowsArgumentException(Func<MethodInfo> func)
    {
        var exception = Assert.Throws<ArgumentException>(() => func());
        Assert.False(string.IsNullOrEmpty(exception.Message));
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Done for testing.")]
    [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Done for testing.")]
    public class TestClass
    {
        public int PublicInstanceMethod() => 42;
        public int PublicInstanceMethod(int a) => a;
        public int PublicInstanceMethod(int a, int b) => a + b;
        public int PublicInstanceMethod(int a, int b, int c) => a + b + c;
        public int PublicInstanceMethod(int a, int b, int c, int d) => a + b + c + d;

        public static int PublicStaticMethod() => 42;
        public static int PublicStaticMethod(int a) => a;
        public static int PublicStaticMethod(int a, int b) => a + b;
        public static int PublicStaticMethod(int a, int b, int c) => a + b + c;
        public static int PublicStaticMethod(int a, int b, int c, int d) => a + b + c + d;

        internal int InternalInstanceMethod() => 42;
        internal int InternalInstanceMethod(int a) => a;
        internal int InternalInstanceMethod(int a, int b) => a + b;
        internal int InternalInstanceMethod(int a, int b, int c) => a + b + c;
        internal int InternalInstanceMethod(int a, int b, int c, int d) => a + b + c + d;

        internal static int InternalStaticMethod() => 42;
        internal static int InternalStaticMethod(int a) => a;
        internal static int InternalStaticMethod(int a, int b) => a + b;
        internal static int InternalStaticMethod(int a, int b, int c) => a + b + c;
        internal static int InternalStaticMethod(int a, int b, int c, int d) => a + b + c + d;

        public int PublicInstanceOverloaded() => 42;

        [SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "Intentional")]
        public int PublicInstanceOverloaded<T>() => 42;
    }
}
