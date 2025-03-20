using Bunit;
using Microsoft.AspNetCore.Components.Web;

namespace Tyne.Blazor;

public class TyneButtonTests : Bunit.TestContext
{
    [Test]
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
        // Button should start enabled
        await Assert.That(buttonElement).IsEnabled();

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        await Assert.That(buttonElement).IsDisabled();

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and be enabled after the click completes
        await Assert.That(buttonElement).IsEnabled();
    }

    [Test]
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
        await Assert.That(buttonElement).IsNotNull();
        // Button should start disabled
        await Assert.That(buttonElement).IsDisabled();

        // Act
        await buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should still be disabled
        await Assert.That(buttonElement).IsDisabled();

        // And the OnClick handler shouldn't have been invoked
        await Assert.That(wasOnClickInvoked).IsFalse();
    }

    [Test]
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
        await Assert.That(buttonElement).IsNotNull();
        // Button should start enabled
        await Assert.That(buttonElement).IsEnabled();

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        await Assert.That(buttonElement).IsDisabled();

        // Update the button to be disabled
        cut.SetParametersAndRender(parameters => parameters
          .Add(p => p.OnClick, onClick)
          .Add(p => p.Disabled, true)
        );

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and still be disabled after the click completes
        await Assert.That(buttonElement).IsDisabled();
    }

    [Test]
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
        await Assert.That(buttonElement).IsNotNull();
        // Button should start enabled
        await Assert.That(buttonElement).IsEnabled();

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        await Assert.That(buttonElement).IsDisabled();

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
        await Assert.That(buttonElement).IsDisabled();

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and be enabled again after the click completes
        await Assert.That(buttonElement).IsEnabled();
    }

    [Test]
    [Arguments(ButtonLockVariant.SpinnerStart, ".tyne-button-locked-progress-circular")]
    [Arguments(ButtonLockVariant.SpinnerEnd, ".tyne-button-locked-progress-circular")]
    [Arguments(ButtonLockVariant.Bar, ".tyne-button-locked-progress-linear")]
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
        await Assert.That(buttonElement).IsNotNull();
        // Button should start enabled
        await Assert.That(buttonElement).IsEnabled();

        // Act
        var clickTask = buttonElement.ClickAsync(new MouseEventArgs());

        // Assert
        // Button should trigger a re-render and be disabled
        cut.WaitForState(() => buttonElement.HasAttribute("disabled"));
        await Assert.That(buttonElement).IsDisabled();

        // Should be showing the loading content
        _ = cut.Find(loadingContentClass);

        // Complete the task
        tcs.SetResult();
        await clickTask;

        // The button should trigger a re-render and be enabled again after the click completes
        await Assert.That(buttonElement).IsEnabled();
    }
}
