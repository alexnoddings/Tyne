using System.Diagnostics.CodeAnalysis;

namespace Tyne.HttpMediator;

public class HttpMediatorOptionsTests
{
    [Fact]
    public void ApiBase_Null_Throws()
    {
        // Arrange
        var options = new HttpMediatorOptions();

        // Act & assert
        Assert.Throws<ArgumentNullException>(() => options.ApiBase = null!);
    }

    [Fact]
    public void ApiBase_Empty_UsesRoot()
    {
        // Arrange
        var options = new HttpMediatorOptions
        {
            // Act
            ApiBase = string.Empty
        };

        // Assert
        Assert.Equal("/", options.ApiBase);
    }

    [Theory]
    [InlineData("some-api/base/_path")]
    [InlineData("/some-api/base/_path")]
    [InlineData("some-api/base/_path/")]
    [InlineData("/some-api/base/_path/")]
    public void ApiBase_CorrectlyFormatsValue(string apiBase)
    {
        const string expected = "/some-api/base/_path/";

        // Arrange
        var options = new HttpMediatorOptions
        {
            // Act
            ApiBase = apiBase
        };

        // Assert
        Assert.Equal(expected, options.ApiBase);
    }

    [Theory]
    [InlineData("with/partial-path")]
    [InlineData("/with/partial-path")]
    [InlineData("/with/partial-path/")]
    [InlineData("with/partial-path/")]
    [SuppressMessage("Design", "CA1054: URI-like parameters should not be strings.", Justification = "Not useful for a test.")]
    public void GetFullApiUri_CorrectlyFormatsValue(string partialUri)
    {
        const string apiBase = "/some-api/base/_path";
        const string expected = $"{apiBase}/with/partial-path";

        // Arrange
        var options = new HttpMediatorOptions
        {
            ApiBase = apiBase
        };

        // Act
        var fullUri = options.GetFullApiUri(partialUri);

        // Assert
        Assert.Equal(expected, fullUri.ToString());
    }
}

// Concrete implementation of abstract base options class
internal sealed class HttpMediatorOptions : HttpMediatorOptionsBase
{
}
