using Microsoft.Extensions.Logging;

namespace Tyne.Blazor.Filtering;

internal static partial class TyneFilteringLoggingExtensions
{
    #region Context
    [LoggerMessage(EventId = 101_001_101, Level = LogLevel.Debug, Message = "Initialising filter context.")]
    public static partial void LogFilterContextInitialising(this ILogger logger);

    [LoggerMessage(EventId = 101_001_102, Level = LogLevel.Error, Message = "Filter context already began initialising.")]
    public static partial void LogFilterContextAlreadyInitialising(this ILogger logger);

    [LoggerMessage(EventId = 101_001_103, Level = LogLevel.Error, Message = "Error initialising filter context.")]
    public static partial void LogFilterContextErrorInitialising(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 101_001_104, Level = LogLevel.Debug, Message = "Ensuring filter values are initialised.")]
    public static partial void LogFilterContextInitialisingValues(this ILogger logger);

    [LoggerMessage(EventId = 101_001_105, Level = LogLevel.Debug, Message = "Notifying controllers attached to '{Key}' of a value update to '{NewValue}'.")]
    public static partial void LogFilterContextNotifyingControllersOfValueUpdate(this ILogger logger, TyneKey key, object? newValue);

    [LoggerMessage(EventId = 101_001_106, Level = LogLevel.Debug, Message = "Configuring request.")]
    public static partial void LogFilterContextConfiguringRequest(this ILogger logger);

    [LoggerMessage(EventId = 101_001_107, Level = LogLevel.Debug, Message = "Reloading context data.")]
    public static partial void LogFilterContextReloadingData(this ILogger logger);

    [LoggerMessage(EventId = 101_001_108, Level = LogLevel.Debug, Message = "Context is in batch update mode, queueing context data reload.")]
    public static partial void LogFilterContextQueueingReloadingDataForBatch(this ILogger logger);

    [LoggerMessage(EventId = 101_001_109, Level = LogLevel.Debug, Message = "Attaching filter value for {FilterKey} ({FilterValueType}).")]
    public static partial void LogFilterContextAttachingFilterValue(this ILogger logger, TyneKey filterKey, Type filterValueType);

    [LoggerMessage(EventId = 101_001_110, Level = LogLevel.Debug, Message = "Attempted to detach filter for {FilterKey}, but no value was attached. Ignoring.")]
    public static partial void LogFilterContextCantDetachFilterValue(this ILogger logger, TyneKey filterKey);

    [LoggerMessage(EventId = 101_001_111, Level = LogLevel.Debug, Message = "Detaching filter for {FilterKey} ({FilterValueType}).")]
    public static partial void LogFilterContextDetachingFilterValue(this ILogger logger, TyneKey filterKey, Type filterValueType);

    [LoggerMessage(EventId = 101_001_112, Level = LogLevel.Debug, Message = "Attaching filter controller for {FilterKey} ({ControllerType}).")]
    public static partial void LogFilterContextAttachingFilterController(this ILogger logger, TyneKey filterKey, Type controllerType);

    [LoggerMessage(EventId = 101_001_113, Level = LogLevel.Debug, Message = "Attempted to detach controller for {FilterKey}, but no controllers are attached. Ignoring.")]
    public static partial void LogFilterContextCantDetachFilterController(this ILogger logger, TyneKey filterKey);

    [LoggerMessage(EventId = 101_001_114, Level = LogLevel.Debug, Message = "Detaching controller for {FilterKey} ({FilterControllerType}).")]
    public static partial void LogFilterContextDetachingFilterController(this ILogger logger, TyneKey filterKey, Type filterControllerType);

    [LoggerMessage(EventId = 101_001_115, Level = LogLevel.Debug, Message = "Starting batch update values.")]
    public static partial void LogFilterContextBatchUpdateValuesStart(this ILogger logger);

    [LoggerMessage(EventId = 101_001_116, Level = LogLevel.Debug, Message = "Flushing batch update values.")]
    public static partial void LogFilterContextBatchUpdateValuesFlush(this ILogger logger);

    [LoggerMessage(EventId = 101_001_117, Level = LogLevel.Debug, Message = "Finished batch update values.")]
    public static partial void LogFilterContextBatchUpdateValuesFinished(this ILogger logger);

    [LoggerMessage(EventId = 101_001_118, Level = LogLevel.Debug, Message = "Hot reload detected, resetting context initialisation state.")]
    public static partial void LogFilterContextHotReloadResetState(this ILogger logger);

    [LoggerMessage(EventId = 101_001_119, Level = LogLevel.Debug, Message = "Re-initialising context after hot reload.")]
    public static partial void LogFilterContextHotReloadReinitialise(this ILogger logger);

    [LoggerMessage(EventId = 101_001_120, Level = LogLevel.Debug, Message = "Notifying controllers attached to '{Key}' of a state change.")]
    public static partial void LogFilterContextNotifyingControllersOfStateChange(this ILogger logger, TyneKey key);
    #endregion

    #region Values
    [LoggerMessage(EventId = 101_001_141, Level = LogLevel.Warning, Message = "Cannot create setter for '{PropertyName}' on '{RequestType}': public instance property  not found.")]
    public static partial void LogFilterValueCreateSetterPropertyNotFound(this ILogger logger, string propertyName, Type requestType);

    [LoggerMessage(EventId = 101_001_142, Level = LogLevel.Warning, Message = "Cannot create setter for '{PropertyName}' on '{RequestType}': value type '{ValueType}' is not assignable to property type '{PropertyType}'.")]
    public static partial void LogFilterValueCreateSetterIncompatibleType(this ILogger logger, string propertyName, Type requestType, Type valueType, Type propertyType);

    [LoggerMessage(EventId = 101_001_143, Level = LogLevel.Warning, Message = "Cannot create setter for '{PropertyName}' on '{RequestType}': property does not have a valid setter.")]
    public static partial void LogFilterValueCreateSetterNoPropertySetter(this ILogger logger, string propertyName, Type requestType);
    #endregion
}
