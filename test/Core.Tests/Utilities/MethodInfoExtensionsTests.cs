using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Tyne.Utilities;

namespace Tyne;

public class MethodInfoExtensionsTests
{
    private const BindingFlags GetMethodBindingFlags =
        BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.Static;

    private static MethodInfo GetMethod(string methodName)
    {
        var method = typeof(MethodInfoExtensionsTests).GetMethod(methodName, GetMethodBindingFlags)!;
        Debug.Assert(method is not null, "Test method not found.", $"Expected test method '{methodName}' not found.");
        return method;
    }

    [Fact]
    public void Instance_Invoke()
    {
        // Arrange
        var method = GetMethod(nameof(InstanceInvoke));

        // Act
        var returned = method.Invoke<int>(this, 42);

        // Assert
        Assert.Equal(42, returned);
    }

    [Fact]
    public async Task Instance_InvokeAsync_Task()
    {
        // Arrange
        var method = GetMethod(nameof(InstanceInvokeTask));

        // Act
        var returned = await method.InvokeAsync<int>(this, 42);

        // Assert
        Assert.Equal(42, returned);
    }

    [Fact]
    public void Static_Invoke()
    {
        // Arrange
        var method = GetMethod(nameof(StaticInvoke));

        // Act
        var returned = method.Invoke<int>(this, 42);

        // Assert
        Assert.Equal(42, returned);
    }

    [Fact]
    public async Task Static_InvokeAsync_Task()
    {
        // Arrange
        var method = GetMethod(nameof(StaticInvokeTask));

        // Act
        var returned = await method.InvokeAsync<int>(this, 42);

        // Assert
        Assert.Equal(42, returned);
    }

    [SuppressMessage("Performance", "CA1822: Mark members as static.", Justification = "This method needs to be instanced for the test.")]
    private int InstanceInvoke(int returnValue) => returnValue;

    [SuppressMessage("Performance", "CA1822: Mark members as static.", Justification = "This method needs to be instanced for the test.")]
    private Task<int> InstanceInvokeTask(int returnValue) => Task.FromResult(returnValue);

    private static int StaticInvoke(int returnValue) => returnValue;
    private static Task<int> StaticInvokeTask(int returnValue) => Task.FromResult(returnValue);
}
