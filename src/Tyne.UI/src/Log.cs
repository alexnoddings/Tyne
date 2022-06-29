using Microsoft.Extensions.Logging;
using Tyne.Results;
using Tyne.UI.Forms;

namespace Tyne.UI;

internal static partial class Log
{
	[LoggerMessage(101_002_001, LogLevel.Error, "Exception while initialising form.", EventName = nameof(FormInitialisingException))]
	public static partial void FormInitialisingException(this ILogger logger, Exception exception);

	[LoggerMessage(101_002_002, LogLevel.Error, "Exception while loading model.", EventName = nameof(FormExceptionLoadingModel))]
	public static partial void FormExceptionLoadingModel(this ILogger logger, Exception exception);

	[LoggerMessage(101_002_003, LogLevel.Debug, "Failure while loading model: {FormLoadFailErrors}", EventName = nameof(FormFailedToLoad))]
	public static partial void FormFailedToLoad(this ILogger logger, IEnumerable<string> formLoadFailErrors);
	public static void FormFailedToLoad<T>(this ILogger logger, Result<T> result)
	{
		var errorMetadataMessages = result.Metadata.OfType<IErrorMetadata>().Select(metadata => metadata.Message).ToList();
		if (errorMetadataMessages.Count == 0)
			FormBadResultNoErrorMetadata(logger);
		FormFailedToLoad(logger, errorMetadataMessages);
	}

	[LoggerMessage(101_002_011, LogLevel.Warning, "Form state transition was invalid ({OldFormState} -> {NewFormState}).", EventName = nameof(FormStateChangeInvalid))]
	public static partial void FormStateChangeInvalid(this ILogger logger, FormState oldFormState, FormState newFormState);

	[LoggerMessage(101_002_012, LogLevel.Warning, "Form result did not contain error metadata.", EventName = nameof(FormBadResultNoErrorMetadata))]
	public static partial void FormBadResultNoErrorMetadata(this ILogger logger);

	[LoggerMessage(101_002_021, LogLevel.Warning, "Form not ready to save: {FormNotReadyToSaveReason}.", EventName = nameof(FormNotReadyToSave))]
	public static partial void FormNotReadyToSave(this ILogger logger, string formNotReadyToSaveReason);

	[LoggerMessage(101_002_022, LogLevel.Debug, "Form not valid.", EventName = nameof(FormNotValid))]
	public static partial void FormNotValid(this ILogger logger);

	[LoggerMessage(101_002_023, LogLevel.Debug, "Form saving.", EventName = nameof(FormSaving))]
	public static partial void FormSaving(this ILogger logger);

	[LoggerMessage(101_002_024, LogLevel.Debug, "Form saved.", EventName = nameof(FormSaved))]
	public static partial void FormSaved(this ILogger logger);

	[LoggerMessage(101_002_025, LogLevel.Error, "Exception while saving form.", EventName = nameof(FormExceptionWhileSaving))]
	public static partial void FormExceptionWhileSaving(this ILogger logger, Exception exception);

	[LoggerMessage(101_002_026, LogLevel.Debug, "Failure while saving form: {FormSaveFailErrors}.", EventName = nameof(FormFailedToSave))]
	public static partial void FormFailedToSave(this ILogger logger, IEnumerable<string> formSaveFailErrors);
	public static void FormFailedToSave<T>(this ILogger logger, Result<T> result)
	{
		var errorMetadataMessages = result.Metadata.OfType<IErrorMetadata>().Select(metadata => metadata.Message).ToList();
		if (errorMetadataMessages.Count == 0)
			FormBadResultNoErrorMetadata(logger);
		FormFailedToSave(logger, errorMetadataMessages);
	}

	[LoggerMessage(101_002_027, LogLevel.Debug, "Invoking form save callback.", EventName = nameof(FormInvokingSaveCallback))]
	public static partial void FormInvokingSaveCallback(this ILogger logger);

	[LoggerMessage(101_002_028, LogLevel.Error, "Exception while invoking form save callback.", EventName = nameof(FormExceptionInvokingSaveCallback))]
	public static partial void FormExceptionInvokingSaveCallback(this ILogger logger, Exception exception);
}
