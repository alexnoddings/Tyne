using System.Diagnostics.Contracts;

namespace Tyne;

public static class Result
{
	/// <summary>
	///		Creates a new Ok <c>Result&lt;<see cref="Unit"/>, <see cref="IHumanError"/>&gt;</c> using <see cref="Unit.Value"/>.
	/// </summary>
	[Pure]
	public static Result<Unit, IHumanError> Ok() =>
		new(true, Unit.Value, default);

	/// <summary>
	///		Creates a new Ok <c>Result&lt;<typeparamref name="T"/>, <see cref="IHumanError"/>&gt;</c> using <paramref name="value"/>.
	/// </summary>
	[Pure]
	public static Result<T, IHumanError> Ok<T>(T value) =>
		new(true, value, default);

	/// <summary>
	///		Creates a new Ok <c>Result&lt;<typeparamref name="T"/>, <typeparamref name="E"/>&gt;</c> using <paramref name="value"/>.
	/// </summary>
	[Pure]
	public static Result<T, E> Ok<T, E>(T value) =>
		new(true, value, default);

	/// <summary>
	///		Creates a new Error <c>Result&lt;<see cref="Unit"/>, <see cref="IHumanError"/>&gt;</c> using <paramref name="error"/>.
	/// </summary>
	[Pure]
	public static Result<Unit, IHumanError> Err(IHumanError error) =>
		new(false, default, error);

	/// <summary>
	///		Creates a new Error <c>Result&lt;<see cref="Unit"/>, <see cref="IHumanError"/>&gt;</c> using a <see cref="HumanError"/> from <paramref name="errorMessage"/>.
	/// </summary>
	[Pure]
	public static Result<Unit, IHumanError> Err(string errorMessage) =>
		new(false, default, new HumanError(errorMessage));

	/// <summary>
	///		Creates a new Error <c>Result&lt;<typeparamref name="T"/>, <see cref="IHumanError"/>&gt;</c> using <paramref name="error"/>.
	/// </summary>
	[Pure]
	public static Result<T, IHumanError> Err<T>(IHumanError error) =>
		new(false, default, error);

	/// <summary>
	///		Creates a new Error <c>Result&lt;<typeparamref name="T"/>, <see cref="IHumanError"/>&gt;</c> using a <see cref="HumanError"/> from <paramref name="errorMessage"/>.
	/// </summary>
	[Pure]
	public static Result<T, IHumanError> Err<T>(string errorMessage) =>
		new(false, default, new HumanError(errorMessage));

	/// <summary>
	///		Creates a new Error <c>Result&lt;<typeparamref name="T"/>, <typeparamref name="E"/>&gt;</c> using <paramref name="error"/>.
	/// </summary>
	[Pure]
	public static Result<T, E> Err<T, E>(E error) =>
		new(false, default, error);

	/// <summary>
	///		Converts a <c>Result&lt;<typeparamref name="TIn"/>, <typeparamref name="EIn"/>&gt;</c> into a <c>Result&lt;<see cref="Unit"/>, <see cref="IHumanError"/>&gt;</c>.
	/// </summary>
	/// <typeparam name="TIn">The input ok type.</typeparam>
	/// <typeparam name="TIn">The input error type. This muust inherit from <typeparamref name="IHumanError"/>.</typeparam>
	/// <returns>
	///		A <c>Result&lt;<typeparamref name="T"/>, <typeparamref name="E"/>&gt;</c> with the same error as <paramref name="result"/>.
	///		The value will be ignored and replaced with <see cref="Unit.Value"/>.
	/// </returns>
	/// <returns></returns>
	/// <remarks>
	///		This is used to convert the <paramref name="result"/> to a result with no value (i.e. <see cref="Unit"/>) while retaining the error,
	///		such as converting <c>Result&lt;SomeType, SomeHumanError&gt;</c> to <c>Result&lt;<see cref="Unit"/>, <see cref="IHumanError"/>&gt;</c>.
	/// </remarks>
	[Pure]
	public static Result<Unit, IHumanError> From<TIn, EIn>(Result<TIn, EIn> result) where EIn : IHumanError =>
		result.IsOk
		? Ok()
		: Err(result.Error);
}
