namespace Tyne;

public class ResultExtensionsOrTests
{
    [Test]
    public async Task OrValue_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var value = 42;

        // Act
        void Act() => _ = result.Or(value);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrValue_NullValue_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<string, int>("okay");
        var errorResult = Result.Error<string, int>(42);
        string value = null!;

        // Act
        void ActOk() => _ = okResult.Or(value);
        void ActError() => _ = errorResult.Or(value);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrValue_OkResult_ReturnsResultValue()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var value = 101;

        // Act
        var or = result.Or(value);

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrValue_ErrorResult_ReturnsOrValue()
    {
        // Arrange
        var result = Result.Error<int, string>("error");
        var value = 101;

        // Act
        var or = result.Or(value);

        // Assert
        await Assert.That(or).IsEqualTo(101);
    }

    [Test]
    public async Task OrValueFactory_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;
        var valueFactory = () => 42;

        // Act
        void Act() => _ = result.Or(valueFactory);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrValueFactory_NullValueFactory_Throws_ArgumentNullException()
    {
        // Arrange
        var okResult = Result.Ok<string, int>("okay");
        var errorResult = Result.Error<string, int>(42);
        Func<string> valueFactory = null!;

        // Act
        void ActOk() => _ = okResult.Or(valueFactory);
        void ActError() => _ = errorResult.Or(valueFactory);

        // Assert
        await Assert.That(ActOk).Throws<ArgumentNullException>();
        await Assert.That(ActError).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrValueFactory_OkResult_ReturnsResultValue()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);
        var valueFactory = Substitute.For<Func<int>>();

        // Act
        var or = result.Or(valueFactory);

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrValueFactory_ErrorResult_ReturnsValueFactory()
    {
        // Arrange
        var result = Result.Error<int, string>("error");
        var valueFactory = Substitute.For<Func<int>>();
        valueFactory.Invoke().Returns(101);

        // Act
        var or = result.Or(valueFactory);

        // Assert
        await Assert.That(or).IsEqualTo(101);
        valueFactory.Received(1).Invoke();
    }

    [Test]
    public async Task OrDefault_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;

        // Act
        void Act() => _ = result.OrDefault();

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrDefault_OkResult_ReturnsResultValue()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);

        // Act
        var or = result.OrDefault();

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrDefault_ErrorResult_ReturnsDefault()
    {
        // Arrange
        var result = Result.Error<int, string>("error");

        // Act
        var or = result.OrDefault();

        // Assert
        await Assert.That(or).IsEqualTo(0);
    }

    [Test]
    public async Task OrNull_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Result<int, int> result = null!;

        // Act
        void Act() => _ = result.OrNull();

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task OrNull_OkResult_ReturnsResultValue()
    {
        // Arrange
        var result = Result.Ok<int, string>(42);

        // Act
        var or = result.OrNull();

        // Assert
        await Assert.That(or).IsEqualTo(42);
    }

    [Test]
    public async Task OrNull_ErrorResult_ReturnsNull()
    {
        // Arrange
        var result = Result.Error<int, string>("error");

        // Act
        var or = result.OrNull();

        // Assert
        await Assert.That(or).IsEqualTo(null);
    }
}
