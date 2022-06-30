using System.Reflection;

namespace Tyne.Utilities;

/// <summary>
///		Helpers for working with <see cref="MethodInfo"/>s.
/// </summary>
public static class MethodHelper
{
	/// <summary>
	///     The default <see cref="BindingFlags"/> used by <see cref="Type.GetMethods(BindingFlags)"/>.
	/// </summary>
	/// <remarks>
	///     When no <see cref="BindingFlags"/> are passed to a <c>Get</c> method, these are used.
	///     They filter to only public, non-inherited members (either instance or static).
	/// </remarks>
	public const BindingFlags DefaultBindingFlags =
		// Do not consider inherited methods by default
		BindingFlags.DeclaredOnly
		// Include instance members
		| BindingFlags.Instance
		// Include static members
		| BindingFlags.Static
		// Include public members
		| BindingFlags.Public;

	/// <summary>
	///		Gets a <see cref="MemberInfo"/> for the <paramref name="methodName"/> method on <typeparamref name="TType"/> 
	///		which takes no parameters.
	/// </summary>
	/// <inheritdoc cref="Get{TType, TParam1, TParam2, TParam3, TParam4}(string, BindingFlags)"/>
	public static MethodInfo Get<TType>(string methodName, BindingFlags bindingFlags = DefaultBindingFlags) =>
		Get(typeof(TType), methodName, bindingFlags);

	/// <summary>
	///		Gets a <see cref="MemberInfo"/> for the <paramref name="methodName"/> method on <typeparamref name="TType"/> 
	///		which takes a parameter of type <typeparamref name="TParam1"/>.
	/// </summary>
	/// <inheritdoc cref="Get{TType, TParam1, TParam2, TParam3, TParam4}(string, BindingFlags)"/>
	public static MethodInfo Get<TType, TParam1>(string methodName, BindingFlags bindingFlags = DefaultBindingFlags) =>
		Get(typeof(TType), methodName, bindingFlags, typeof(TParam1));

	/// <summary>
	///		Gets a <see cref="MemberInfo"/> for the <paramref name="methodName"/> method on <typeparamref name="TType"/> 
	///		which takes parameters of type <typeparamref name="TParam1"/>, and <typeparamref name="TParam2"/>.
	/// </summary>
	/// <inheritdoc cref="Get{TType, TParam1, TParam2, TParam3, TParam4}(string, BindingFlags)"/>
	public static MethodInfo Get<TType, TParam1, TParam2>(string methodName, BindingFlags bindingFlags = DefaultBindingFlags) =>
		Get(typeof(TType), methodName, bindingFlags, typeof(TParam1), typeof(TParam2));

	/// <summary>
	///		Gets a <see cref="MemberInfo"/> for the <paramref name="methodName"/> method on <typeparamref name="TType"/> 
	///		which takes parameters of type <typeparamref name="TParam1"/>, <typeparamref name="TParam2"/>, and <typeparamref name="TParam3"/>.
	/// </summary>
	/// <inheritdoc cref="Get{TType, TParam1, TParam2, TParam3, TParam4}(string, BindingFlags)"/>
	public static MethodInfo Get<TType, TParam1, TParam2, TParam3>(string methodName, BindingFlags bindingFlags = DefaultBindingFlags) =>
		Get(typeof(TType), methodName, bindingFlags, typeof(TParam1), typeof(TParam2), typeof(TParam3));

	/// <summary>
	///		Gets a <see cref="MemberInfo"/> for the <paramref name="methodName"/> method on <typeparamref name="TType"/> 
	///		which takes parameters of type <typeparamref name="TParam1"/>, <typeparamref name="TParam2"/>, <typeparamref name="TParam3"/>, and <typeparamref name="TParam4"/>.
	/// </summary>
	/// <typeparam name="TType">
	///		The type which the method is declared on.
	///		This ignores inherited members when <paramref name="bindingFlags"/> has <see cref="BindingFlags.DeclaredOnly"/> set.
	///		This is set by the <see cref="DefaultBindingFlags"/> used when no <paramref name="bindingFlags"/> are specified.
	///	</typeparam>
	/// <typeparam name="TParam1">The type of the first parameter.</typeparam>
	/// <typeparam name="TParam2">The type of the second parameter.</typeparam>
	/// <typeparam name="TParam3">The type of the third parameter.</typeparam>
	/// <typeparam name="TParam4">The type of the fourth parameter.</typeparam>
	/// <inheritdoc cref="Get(Type, string, BindingFlags, Type[])"/>
	public static MethodInfo Get<TType, TParam1, TParam2, TParam3, TParam4>(string methodName, BindingFlags bindingFlags = DefaultBindingFlags) =>
		Get(typeof(TType), methodName, bindingFlags, typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4));

	/// <inheritdoc cref="Get(Type, string, BindingFlags, Type[])"/>
	public static MethodInfo Get(Type declaringType, string methodName, params Type[] parameterTypes) =>
		Get(declaringType, methodName, DefaultBindingFlags, parameterTypes);

	/// <summary>
	///		Gets a <see cref="MemberInfo"/> for the <paramref name="methodName"/> method on <paramref name="declaringType"/> which takes parameters of types <paramref name="parameterTypes"/>.
	/// </summary>
	/// <remarks>
	///	<para>
	///		It is recommended to store the <see cref="MethodInfo"/> in a <see langword="static"/> variable.
	///		Methods don't change signature at runtime, so it is inefficient to perform this reflection every
	///		time a method it needed.
	///	</para>
	///	<para>
	///		If calculated in a <see langword="static"/> context, this can render the declaring <see cref="Type"/> unusable
	///		at runtime if the method is not found. This will result in a <see cref="TypeInitializationException"/>.
	///		See <see href="https://docs.microsoft.com/en-us/dotnet/api/system.typeinitializationexception#Static">the docs</see> for more info.
	///	</para>
	/// </remarks>
	/// <param name="declaringType">
	///		The type which the method is declared on.
	///		This ignores inherited members when <paramref name="bindingFlags"/> has <see cref="BindingFlags.DeclaredOnly"/> set.
	///		This is set by the <see cref="DefaultBindingFlags"/> used when no <paramref name="bindingFlags"/> are specified.
	///	</param>
	/// <param name="methodName">The name of the method.</param>
	/// <param name="bindingFlags">The binding flags to use when searching for the method. Defaults to using <see cref="DefaultBindingFlags"/> when unspecified.</param>
	/// <param name="parameterTypes">The types which the method takes.</param>
	/// <returns>A <see cref="MethodInfo"/> of the specified method.</returns>
	/// <exception cref="ArgumentException">When no such method could be found.</exception>
	public static MethodInfo Get(Type declaringType, string methodName, BindingFlags bindingFlags = DefaultBindingFlags, params Type[] parameterTypes)
	{
		// The names of the parameter types.
		// These are calculated by Name rather than FullName or AssemblyQualifiedName
		// as these can be null for generic parameters.
		IEnumerable<string> parameterTypeNames = parameterTypes.Select(type => type.Name);

		MethodInfo[] methods =
			declaringType
			// Get all methods bound by bindingFlags
			.GetMethods(bindingFlags)
			.Where(method =>
				// Whose names equal the methodName
				method.Name == methodName
				&& method
				// Whose parameters types names are equal to those in parameterTypes
				.GetParameters()
				.Select(parameter => parameter.ParameterType.Name)
				.SequenceEqual(parameterTypeNames))
				.ToArray();

		if (methods.Length == 0)
			throw new ArgumentException($"No {methodName} method found on {declaringType.Name} with parameters [{string.Join(", ", parameterTypes.Select(type => type.Name))}].");

		if (methods.Length > 1)
			throw new ArgumentException($"Multiple {methodName} methods found on {declaringType.Name} with parameters [{string.Join(", ", parameterTypes.Select(type => type.Name))}].");

		return methods[0];
	}
}
