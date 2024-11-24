using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Tyne.HttpMediator.Client;

internal static partial class FluentValidationMiddlewareLogging
{
    private static readonly Func<ILogger, IDisposable?> _validationScope =
        LoggerMessage.DefineScope("Validating request");

    public static IDisposable? BeginValidationScope(this ILogger logger) =>
        _validationScope(logger);

    [LoggerMessage(EventId = 101_004_000, Level = LogLevel.Trace, Message = "No validators registered.")]
    public static partial void LogNoValidators(this ILogger logger);

    private static readonly Action<ILogger, Type, Exception?> _logExecutingValidator =
        LoggerMessage.Define<Type>(
            LogExecutingValidatorLevel,
            new EventId(101_004_001, nameof(LogExecutingValidator)),
            "Executing validator {Validator}.",
            new LogDefineOptions
            {
                SkipEnabledCheck = true
            }
        );

    private const LogLevel LogExecutingValidatorLevel = LogLevel.Trace;
    public static void LogExecutingValidator(this ILogger logger, IValidator validator)
    {
        // Implemented manually to avoid calling validator.GetType if not needed
        if (logger.IsEnabled(LogExecutingValidatorLevel))
        {
            var type = validator.GetType();
            _logExecutingValidator(logger, type, null);
        }
    }

    private static readonly Action<ILogger, string[], Exception?> _logValidationFailure =
        LoggerMessage.Define<string[]>(
            LogValidationFailureLevel,
            new EventId(101_004_002, nameof(LogValidationFailure)),
            "Validation failed: {ValidationFailures}.",
            new LogDefineOptions
            {
                SkipEnabledCheck = true
            }
        );

    private const LogLevel LogValidationFailureLevel = LogLevel.Debug;
    public static void LogValidationFailure(this ILogger logger, ValidationResult validationResult)
    {
        // Implemented manually to avoid allocating if not needed
        if (logger.IsEnabled(LogValidationFailureLevel))
        {
            var errorMessages = validationResult.Errors.Select(failure => failure.ErrorMessage).ToArray();
            _logValidationFailure(logger, errorMessages, null);
        }
    }
}
