using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components.Web;

namespace Tyne.Blazor;

public class TyneButtonTests : TestContext
{
    private static class AssertButton
    {
        public static void Enabled(IElement buttonElement) =>
            Assert.False(buttonElement.Matches("[disabled=\"\"]"), "Button should not be disabled.");

        public static void Disabled(IElement buttonElement) =>
            Assert.True(buttonElement.Matches("[disabled=\"\"]"), "Button should be disabled.");
    }

    [Fact]
    public async Task Enabled_Click_LocksButton()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        Task onClick() => tcs.Task;

        var cut = RenderComponent<TyneButton>(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, false)
        );

        var buttonElement = cut.Find("button");
        Assert.NotNull(buttonElement);
        // Button should start enabled
        AssertButton.Enabled(buttonElement);

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        AssertButton.Disabled(buttonElement);

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and be enabled after the click completes
        AssertButton.Enabled(buttonElement);
    }

    [Fact]
    public async Task Disabled_Click_DoesNothing()
    {
        // Arrange
        var wasOnClickInvoked = false;
        Task onClick()
        {
            wasOnClickInvoked = true;
            return Task.CompletedTask;
        }

        var cut = RenderComponent<TyneButton>(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, true)
        );

        var buttonElement = cut.Find("button");
        Assert.NotNull(buttonElement);
        // Button should start disabled
        AssertButton.Disabled(buttonElement);

        // Act
        await buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should still be disabled
        AssertButton.Disabled(buttonElement);

        // And the OnClick handler shouldn't have been invoked
        Assert.False(wasOnClickInvoked);
    }

    [Fact]
    public async Task StartsEnabled_DisabledDuringClick_EndsDisabled()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        Task onClick() => tcs.Task;

        var cut = RenderComponent<TyneButton>(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, false)
        );

        var buttonElement = cut.Find("button");
        Assert.NotNull(buttonElement);
        // Button should start enabled
        AssertButton.Enabled(buttonElement);

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        AssertButton.Disabled(buttonElement);

        // Update the button to be disabled
        cut.SetParametersAndRender(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, true)
        );

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and still be disabled after the click completes
        AssertButton.Disabled(buttonElement);
    }

    [Fact]
    public async Task Enabled_DisabledChangesDuringClick_EndsEnabled()
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        Task onClick() => tcs.Task;

        var cut = RenderComponent<TyneButton>(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, false)
        );

        var buttonElement = cut.Find("button");
        Assert.NotNull(buttonElement);
        // Button should start enabled
        AssertButton.Enabled(buttonElement);

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        AssertButton.Disabled(buttonElement);

        // Update the button to be disabled
        cut.SetParametersAndRender(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, true)
        );

        // Then update the button to be enabled again
        cut.SetParametersAndRender(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, false)
        );

        // The button should still be disabled
        AssertButton.Disabled(buttonElement);

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and be enabled again after the click completes
        AssertButton.Enabled(buttonElement);
    }

    [Theory]
    [InlineData(ButtonLockVariant.SpinnerStart, ".tyne-button-locked-progress-circular")]
    [InlineData(ButtonLockVariant.SpinnerEnd, ".tyne-button-locked-progress-circular")]
    [InlineData(ButtonLockVariant.Bar, ".tyne-button-locked-progress-linear")]
    public async Task Click_ShowsLoadingContent(ButtonLockVariant buttonLockVariant, string loadingContentClass)
    {
        // Arrange
        var tcs = new TaskCompletionSource();
        Task onClick() => tcs.Task;

        var cut = RenderComponent<TyneButton>(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.LockVariant, buttonLockVariant)
        );

        var buttonElement = cut.Find("button");
        Assert.NotNull(buttonElement);
        // Button should start enabled
        AssertButton.Enabled(buttonElement);

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        AssertButton.Disabled(buttonElement);

        // Should be showing the loading content
        cut.Find(loadingContentClass);

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and be enabled again after the click completes
        AssertButton.Enabled(buttonElement);
    }
}
