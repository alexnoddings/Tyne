namespace Tyne;

public class OptionExtensionsSelectTests
{
    #region SyncResult, SyncSelector
    [Test]
    public async Task Select_SyncResult_SyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();
        Func<int, string> selector = null!;

        // Act
        void ActSome() => someOption.Select(selector);
        void ActNone() => noneOption.Select(selector);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_SomeOption_CallsSelectorOnce()
    {
        // Arrange
        var option = Option.Some(42);
        var selector = Substitute.For<Func<int, long>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(101);

        // Act
        var newOption = option.Select(selector);

        // Assert
        await Assert.That(newOption).IsSome(101);
        selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_SomeOption_SelectorReturnsNull_ReturnsNone()
    {
        // Arrange
        var option = Option.Some(42);
        static object selector(int _) => null!;

        // Act
        var newOption = option.Select(selector);

        // Assert
        await Assert.That(newOption).IsNone();
    }

    [Test]
    public async Task Select_SyncResult_SyncSelector_NoneOption_DoesNotCallSelector()
    {
        // Arrange
        var option = Option.None<int>();
        var selector = Substitute.For<Func<int, long>>();

        // Act
        var newOption = option.Select(selector);

        // Assert
        await Assert.That(newOption).IsNone();
        selector.DidNotReceive().Invoke(Arg.Any<int>());
    }
    #endregion

    #region SyncResult, AsyncSelector
    [Test]
    public async Task Select_SyncResult_AsyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var someOption = Option.Some(42);
        var noneOption = Option.None<int>();
        Func<int, Task<string>> selector = null!;

        // Act
        void ActSome() => _ = someOption.Select(selector);
        void ActNone() => _ = noneOption.Select(selector);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_SyncResult_AsyncSelector_SomeOption_CallsSelectorOnce()
    {
        // Arrange
        var option = Option.Some(42);
        var selector = Substitute.For<Func<int, Task<long>>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(Task.FromResult(101L));

        // Act
        var newOption = await option.Select(selector);

        // Assert
        await Assert.That(newOption).IsSome(101);
        _ = selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_SyncResult_AsyncSelector_NoneOption_DoesNotCallSelector()
    {
        // Arrange
        var option = Option.None<int>();
        var selector = Substitute.For<Func<int, Task<long>>>();

        // Act
        var newOption = option.Select(selector);

        // Assert
        await Assert.That(newOption).IsNone();
    }
    #endregion

    #region AsyncOption, SyncSelector
    [Test]
    public async Task Select_AsyncOption_SyncSelector_NullResult_Throws_ArgumentNullException()
    {
        // Arrange
        Task<Option<int>> optionTask = null!;
        var selector = Substitute.For<Func<int, string>>();

        // Act
        void Act() => _ = optionTask.Select(selector);

        // Assert
        await Assert.That(Act).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncOption_SyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var someOptionTask = Option.Some(42).ToTask();
        var noneOptionTask = Option.None<int>().ToTask();
        Func<int, string> selector = null!;

        // Act
        void ActSome() => _ = someOptionTask.Select(selector);
        void ActNone() => _ = noneOptionTask.Select(selector);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncOption_SyncSelector_SomeOption_CallsSelectorOnce()
    {
        // Arrange
        var optionTask = Option.Some(42).ToTask();
        var selector = Substitute.For<Func<int, long>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(101);

        // Act
        var newOption = await optionTask.Select(selector);

        // Assert
        await Assert.That(newOption).IsSome(101);
        selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_AsyncOption_SyncSelector_NoneOption_DoesNotCallSelector()
    {
        // Arrange
        var optionTask = Option.None<int>().ToTask();
        var selector = Substitute.For<Func<int, long>>();

        // Act
        var newOption = await optionTask.Select(selector);

        // Assert
        await Assert.That(newOption).IsNone();
    }
    #endregion

    #region AsyncOption, AsyncSelector
    [Test]
    public async Task Select_AsyncOption_AsyncSelector_NullSelector_Throws_ArgumentNullException()
    {
        // Arrange
        var someOptionTask = Option.Some(42).ToTask();
        var noneOptionTask = Option.None<int>();
        Func<int, Task<string>> selector = null!;

        // Act
        void ActSome() => _ = someOptionTask.Select(selector);
        void ActNone() => _ = noneOptionTask.Select(selector);

        // Assert
        await Assert.That(ActSome).Throws<ArgumentNullException>();
        await Assert.That(ActNone).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Select_AsyncOption_AsyncSelector_SomeOption_CallsSelectorOnce()
    {
        // Arrange
        var optionTask = Option.Some(42).ToTask();
        var selector = Substitute.For<Func<int, Task<long>>>();
        _ = selector.Invoke(Arg.Is(42)).Returns(Task.FromResult(101L));

        // Act
        var newOption = optionTask.Select(selector);

        // Assert
        await Assert.That(newOption).IsSome(101);
        _ = selector.Received(1).Invoke(Arg.Is(42));
    }

    [Test]
    public async Task Select_AsyncOption_AsyncSelector_NoneOption_DoesNotCallSelector()
    {
        // Arrange
        var optionTask = Option.None<int>().ToTask();
        var selector = Substitute.For<Func<int, Task<long>>>();

        // Act
        var newOption = await optionTask.Select(selector);

        // Assert
        await Assert.That(newOption).IsNone();
    }
    #endregion
}
