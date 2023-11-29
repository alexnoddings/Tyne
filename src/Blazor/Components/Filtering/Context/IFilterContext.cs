using Tyne.Blazor.Filtering.Controllers;
using Tyne.Blazor.Filtering.Values;
using Tyne.Blazor.Persistence;

namespace Tyne.Blazor.Filtering.Context;

/// <summary>
///     The context which Tyne rich interactive filtering runs inside of.
///     Each context (e.g. a table) will have it's own context.
/// </summary>
/// <typeparam name="TRequest">The type of request which loads data in the context.</typeparam>
/// <seealso href="/docs/packages/Blazor/Tables/Lifecycle.html">The docs on filtering for a more comprehensive overview.</seealso>
public interface IFilterContext<out TRequest>
{
    /// <summary>
    ///     Provides persistence for filter values in the context.
    /// </summary>
    /// <remarks>
    ///     You should use this over <c>[Inject]</c>ing it as the context may alter it's behaviour,
    ///     such as during <see cref="BatchUpdateValuesAsync(Func{Task})"/>.
    /// </remarks>
    public IUrlPersistenceService Persistence { get; }

    /// <summary>
    ///     Executes a batch update of values within the context.
    /// </summary>
    /// <param name="func">
    ///     What to execute during the batch value update.
    /// </param>
    /// <returns>A <see cref="Task"/> representing <paramref name="func"/> being executed.</returns>
    /// <remarks>
    ///     <para>
    ///         This is required when updating more than one value at once.
    ///         See the remarks for <see cref="IUrlPersistenceService"/> for more info on why.
    ///     </para>
    ///     <para>
    ///         Only one batch update may be executed at a time.
    ///     </para>
    ///     <para>
    ///         If <paramref name="func"/> throws an exception, the batch update will not attempt to execute.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When trying to execute more than one batch update simultaneously.</exception>
    public Task BatchUpdateValuesAsync(Func<Task> func);

    /// <summary>
    ///     Attaches a <paramref name="filter"/> representing <paramref name="key"/> to the context.
    /// </summary>
    /// <typeparam name="TValue">The type of value being filtered.</typeparam>
    /// <param name="key">A <see cref="TyneKey"/> which the filter represents. This may not be <see cref="TyneKey.Empty"/>.</param>
    /// <param name="filter">A filter value instance.</param>
    /// <returns>
    ///     An <see cref="IFilterValueHandle{TValue}"/>.
    ///     This is how filters should interact with the context once attached,
    ///     such as reloading the data or notifying it of a value change.
    ///     It should be disposed of to detach the <paramref name="filter"/> once it goes out of scope.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         The returned handle must be disposed of once the <paramref name="filter"/>
    ///         goes out of scope to ensure that it is properly detached from the context.
    ///     </para>
    ///     <para>
    ///         Filters can only be attached during context initialisation.
    ///     </para>
    ///     <para>
    ///         See <see cref="IFilterValueHandle{TValue}"/>'s docs for what filters can do once attached to the context.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When trying to attach a value after the context is initialised.</exception>
    /// <exception cref="KeyEmptyException">When <paramref name="key"/> is empty.</exception>
    public IFilterValueHandle<TValue> AttachValue<TValue>(TyneKey key, IFilterValue<TRequest, TValue> filter);

    /// <summary>
    ///     Attaches a <paramref name="controller"/> for <paramref name="key"/> to the context.
    /// </summary>
    /// <typeparam name="TValue">The type of value being filtered.</typeparam>
    /// <param name="key">A <see cref="TyneKey"/> which the controller should attach to.</param>
    /// <param name="controller">A controller instance.</param>
    /// <returns>
    ///     An <see cref="IFilterControllerHandle{TValue}"/>.
    ///     This is how controllers should interact with the context once attached,
    ///     such as by accessing the <see cref="IFilterValue{TRequest, TValue}"/> represented by <paramref name="key"/>.
    ///     It should be disposed of to detach the <paramref name="controller"/> once it goes out of scope.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         The returned handle must be disposed of once the <paramref name="controller"/>
    ///         goes out of scope to ensure that it is properly detached from the context.
    ///     </para>
    ///     <para>
    ///         This method should only be called once all values have been attached.
    ///         An exception will be thrown if a controller tries to attach to a key before a value is attached to it.
    ///     </para>
    ///     <para>
    ///         Unlike filter values, controllers may be attached after context initialisation.
    ///     </para>
    ///     <para>
    ///         See <see cref="IFilterValueHandle{TValue}"/>'s docs for what controllers can do once attached to the context.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">When trying to attach a controller when no value is attached for <paramref name="key"/>.</exception>
    /// <exception cref="ArgumentException">When trying to attach a controller whose <typeparamref name="TValue"/> does not match the value attached to <paramref name="key"/>.</exception>
    /// <exception cref="KeyEmptyException">When <paramref name="key"/> is empty.</exception>
    public IFilterControllerHandle<TValue> AttachController<TValue>(TyneKey key, IFilterController<TValue> controller);
}
