namespace Tyne;

// Castle (and thus NSubstitute) don't like intercepting calls to object as some of their internals use methods like GetHashCode.
// Instead, we use a mock object with known behaviour for testing Object-related methods.
internal sealed class MockObject
{
    public const int HashCode = 101;
    public int GetHashCodeInvocationCount { get; private set; }

    public override int GetHashCode()
    {
        GetHashCodeInvocationCount++;
        return HashCode;
    }

    public const string AsString = "xyz";
    public int ToStringInvocationCount { get; private set; }

    public override string? ToString()
    {
        ToStringInvocationCount++;
        return AsString;
    }
}
