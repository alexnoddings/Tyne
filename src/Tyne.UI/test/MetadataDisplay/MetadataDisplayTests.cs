using Microsoft.AspNetCore.Components.Web;
using Tyne.Results;

namespace Tyne.UI;

public class MetadataDisplayTests : TestContext
{
	public static object[][] MessagesData() => new[]
	{
		// Single metadata
		new object[] { Result.Successful(new SuccessMetadata("Example success message.")), new string[] { "Example success message." } },
		new object[] { Result.Successful(new InfoMetadata("Example info message.")), new string[] { "Example info message." } },
		new object[] { Result.Successful(new ErrorMetadata("Example error message.")), new string[] { "Example error message." } },

		// Multiple metadatas
		new object[] {
			Result.Successful(
				   new SuccessMetadata("Example success message #1"), new SuccessMetadata("Example success message #2"),
				   new InfoMetadata("Example info message #1"), new InfoMetadata("Example info message #2"),
				   new ErrorMetadata("Example error message #1"), new ErrorMetadata("Example error message #2")
			),
			new string[] {
				"Example success message #1", "Example success message #2",
				"Example info message #1", "Example info message #2",
				"Example error message #1", "Example error message #2"
			}
		},
	};

	[Theory]
	[MemberData(nameof(MessagesData))]
	public void Enabled_ShouldShowMessages(Result<Unit> result, string[] expectedMessages)
	{
		// Act
		var cut = RenderComponent<TyneMetadataDisplay>(parameters => parameters
			.Add(metadataDisplay => metadataDisplay.Disabled, false)
			.Add(metadataDisplay => metadataDisplay.Dismissible, false)
			.Add(metadataDisplay => metadataDisplay.Metadata, result.Metadata)
		);

		// Assert
		foreach (var expectedMessage in expectedMessages)
			Assert.Contains(expectedMessage, cut.Markup);
	}

	[Theory]
	[MemberData(nameof(MessagesData))]
	public void Disabled_ShouldNotShowMessages(Result<Unit> result, string[] expectedMessages)
	{
		// Act
		var cut = RenderComponent<TyneMetadataDisplay>(parameters => parameters
			.Add(metadataDisplay => metadataDisplay.Disabled, true)
			.Add(metadataDisplay => metadataDisplay.Dismissible, false)
			.Add(metadataDisplay => metadataDisplay.Metadata, result.Metadata)
		);

		// Assert
		foreach (var expectedMessage in expectedMessages)
			Assert.DoesNotContain(expectedMessage, cut.Markup);
	}

	public static object[][] DismissibleResultsData() => new[]
	{
		// Single metadata
		new object[] { Result.Successful(new SuccessMetadata("Example success message.")) },
		new object[] { Result.Successful(new InfoMetadata("Example info message.")) },
		new object[] { Result.Successful(new ErrorMetadata("Example error message.")) },

		// Multiple metadatas
		new object[] {
			Result.Successful(
				   new SuccessMetadata("Example success message #1"), new SuccessMetadata("Example success message #2"),
				   new InfoMetadata("Example info message #1"), new InfoMetadata("Example info message #2"),
				   new ErrorMetadata("Example error message #1"), new ErrorMetadata("Example error message #2")
			)
		},
	};

	[Theory]
	[MemberData(nameof(DismissibleResultsData))]
	public async Task Dismissible_ShouldRemoveMetadata(Result<Unit> result)
	{
		// The original metadata
		var originalMetadata = result.Metadata.ToList();

		// Arrange
		// Clears metadata of type T from result
		async Task ClearMessagesAsync<T>(IRenderedComponent<TyneMetadataDisplay> cut, string alertCssSelector) where T : IMessageMetadata
		{
			// Return if the result doesn't have this T metadata
			var doesHaveRelevantMetadata = result.Metadata.OfType<T>().Any();
			if (!doesHaveRelevantMetadata)
				return;

			// Find the alert component
			var alertComponent = cut.Find(alertCssSelector);
			Assert.NotNull(alertComponent);

			// Dismiss the type of alert
			var closeAlertButton = alertComponent.QuerySelector(".mud-alert-close button.mud-button-root");
			Assert.NotNull(closeAlertButton);
			await closeAlertButton!.ClickAsync(new MouseEventArgs());

			// Loop over every original metadata T
			foreach (var metadata in originalMetadata!.OfType<T>())
			{
				// Metadata should have been removed from Result.Metadata
				Assert.DoesNotContain(metadata, result.Metadata);
				// Component should have updated to not render the metadata
				Assert.DoesNotContain(metadata.Message, cut.Markup, StringComparison.OrdinalIgnoreCase);
			}
		}

		// Act
		var cut = RenderComponent<TyneMetadataDisplay>(parameters => parameters
			.Add(metadataDisplay => metadataDisplay.Disabled, false)
			.Add(metadataDisplay => metadataDisplay.Dismissible, true)
			.Add(metadataDisplay => metadataDisplay.Metadata, result.Metadata)
		);

		// Assert
		// Clear messages of each type individually
		await ClearMessagesAsync<ISuccessMetadata>(cut, ".mud-alert-outlined-success");
		await ClearMessagesAsync<IInfoMetadata>(cut, ".mud-alert-outlined-info");
		await ClearMessagesAsync<IErrorMetadata>(cut, ".mud-alert-outlined-error");

		// Should be no message success, info, or error metadata left
		Assert.Empty(result.Metadata.OfType<ISuccessMetadata>());
		Assert.Empty(result.Metadata.OfType<IInfoMetadata>());
		Assert.Empty(result.Metadata.OfType<IErrorMetadata>());

		// And the component shouldn't render at all
		Assert.Empty(cut.Markup);
	}
}
