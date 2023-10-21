using Microsoft.Extensions.Logging;

namespace Tyne.Blazor.Filtering;

internal static partial class TyneFilteringLoggingExtensions
{
    #region Context
    [LoggerMessage(EventId = 101_004_001, Level = LogLevel.Debug, Message = "Initialising filter context.")]
    public static partial void LogFilterContextInitialising(this ILogger logger);

    [LoggerMessage(EventId = 101_004_002, Level = LogLevel.Error, Message = "Filter context already began initialising.")]
    public static partial void LogFilterContextAlreadyInitialising(this ILogger logger);

    [LoggerMessage(EventId = 101_004_003, Level = LogLevel.Error, Message = "Error initialising filter context.")]
    public static partial void LogFilterContextErrorInitialising(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_004_004, Level = LogLevel.Debug, Message = "Ensuring filter values are initialised.")]
    public static partial void LogFilterContextInitialisingValues(this ILogger logger);

    [LoggerMessage(EventId = 101_004_005, Level = LogLevel.Debug, Message = "Notifying controllers attached to '{Key}' of a value update to '{NewValue}'.")]
    public static partial void LogFilterContextNotifyingControllersOfValueUpdate(this ILogger logger, string key, object? newValue);

    [LoggerMessage(EventId = 101_004_006, Level = LogLevel.Debug, Message = "Configuring request.")]
    public static partial void LogFilterContextConfiguringRequest(this ILogger logger);

    [LoggerMessage(EventId = 101_004_007, Level = LogLevel.Debug, Message = "Reloading context data.")]
    public static partial void LogFilterContextReloadingData(this ILogger logger);

    [LoggerMessage(EventId = 101_004_008, Level = LogLevel.Debug, Message = "Context is in batch update mode, queueing context data reload.")]
    public static partial void LogFilterContextQueueingReloadingDataForBatch(this ILogger logger);

    [LoggerMessage(EventId = 101_004_009, Level = LogLevel.Debug, Message = "Attaching filter value for {FilterKey} ({FilterValueType}).")]
    public static partial void LogFilterContextAttachingFilterValue(this ILogger logger, string filterKey, Type filterValueType);

    [LoggerMessage(EventId = 101_004_010, Level = LogLevel.Debug, Message = "Attempted to detach filter for {FilterKey}, but no value was attached. Ignoring.")]
    public static partial void LogFilterContextCantDetachFilterValue(this ILogger logger, string filterKey);

    [LoggerMessage(EventId = 101_004_011, Level = LogLevel.Debug, Message = "Detaching filter for {FilterKey} ({FilterValueType}).")]
    public static partial void LogFilterContextDetachingFilterValue(this ILogger logger, string filterKey, Type filterValueType);

    [LoggerMessage(EventId = 101_004_012, Level = LogLevel.Debug, Message = "Attaching filter controller for {FilterKey} ({ControllerType}).")]
    public static partial void LogFilterContextAttachingFilterController(this ILogger logger, string filterKey, Type controllerType);

    [LoggerMessage(EventId = 101_004_013, Level = LogLevel.Debug, Message = "Attempted to detach controller for {FilterKey}, but no controllers are attached. Ignoring.")]
    public static partial void LogFilterContextCantDetachFilterController(this ILogger logger, string filterKey);

    [LoggerMessage(EventId = 101_004_014, Level = LogLevel.Debug, Message = "Detaching controller for {FilterKey} ({FilterControllerType}).")]
    public static partial void LogFilterContextDetachingFilterController(this ILogger logger, string filterKey, Type filterControllerType);

    [LoggerMessage(EventId = 101_004_015, Level = LogLevel.Debug, Message = "Starting batch update values.")]
    public static partial void LogFilterContextBatchUpdateValuesStart(this ILogger logger);

    [LoggerMessage(EventId = 101_004_016, Level = LogLevel.Debug, Message = "Flushing batch update values.")]
    public static partial void LogFilterContextBatchUpdateValuesFlush(this ILogger logger);

    [LoggerMessage(EventId = 101_004_017, Level = LogLevel.Debug, Message = "Finished batch update values.")]
    public static partial void LogFilterContextBatchUpdateValuesFinished(this ILogger logger);

    [LoggerMessage(EventId = 101_004_018, Level = LogLevel.Debug, Message = "Hot reload detected, resetting context initialisation state.")]
    public static partial void LogFilterContextHotReloadResetState(this ILogger logger);

    [LoggerMessage(EventId = 101_004_019, Level = LogLevel.Debug, Message = "Re-initialising context after hot reload.")]
    public static partial void LogFilterContextHotReloadReinitialise(this ILogger logger);
    #endregion
}
