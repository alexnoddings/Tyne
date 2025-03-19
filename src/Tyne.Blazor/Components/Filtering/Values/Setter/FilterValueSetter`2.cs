namespace Tyne.Blazor.Filtering.Values;

/// <summary>
///     A delegate which, when executed, updates a value on <paramref name="request"/> with <paramref name="value"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request.</typeparam>
/// <typeparam name="TValue">The type of value on <typeparamref name="TRequest"/>.</typeparam>
/// <param name="request">The request object to set the <paramref name="value"/> on.</param>
/// <param name="value">The <typeparamref name="TValue"/> to set on the <paramref name="request"/>.</param>
/// <remarks>
///     This is an abstraction designed for use with <see cref="TyneFilterValueBase{TRequest, TValue}"/>
///     to define how values get set during filter configuration.
/// </remarks>

public delegate void FilterValueSetter<in TRequest, in TValue>(TRequest request, TValue value);
