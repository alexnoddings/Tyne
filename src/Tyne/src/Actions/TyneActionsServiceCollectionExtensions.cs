using System.Reflection;
using Tyne.Actions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions methods for adding <see cref="IAction{TInput, TOutput}"/>s to an <see cref="IServiceCollection"/>.
/// </summary>
public static class TyneActionsServiceCollectionExtensions
{
	/// <summary>
	///     Adds <see cref="IAction{TInput, TOutput}"/>s from the <see cref="Assembly"/> containing <typeparamref name="T"/>.
	///     Actions have a <see cref="ServiceLifetime.Scoped"/> lifetime by default.
	///     Uses <see cref="RegisterActionsMode.Implicit"/>.
	/// </summary>
	/// <typeparam name="T">Any type in the <see cref="Assembly"/> to scan.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the actions to.</param>
	/// <returns><paramref name="services"/> to allow for chaining.</returns>
	public static IServiceCollection AddActions<T>(this IServiceCollection services) =>
		services.AddActions(typeof(T).Assembly);

	/// <summary>
	///     Adds <see cref="IAction{TInput, TOutput}"/>s from <paramref name="assembly"/> and <paramref name="otherAssemblies"/>.
	///     Actions have a <see cref="ServiceLifetime.Scoped"/> lifetime by default.
	///     Uses <see cref="RegisterActionsMode.Implicit"/>.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the actions to.</param>
	/// <param name="assembly">The first <see cref="Assembly"/> to scan.</param>
	/// <param name="otherAssemblies">The other <see cref="Assembly"/>s to scan.</param>
	/// <returns><paramref name="services"/> to allow for chaining.</returns>
	public static IServiceCollection AddActions(this IServiceCollection services, Assembly assembly, params Assembly[] otherAssemblies) =>
		services.AddActions(ServiceLifetime.Scoped, RegisterActionsMode.Implicit, assembly, otherAssemblies);

	/// <summary>
	///     Adds <see cref="IAction{TInput, TOutput}"/>s from the <see cref="Assembly"/> containing <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">Any type in the <see cref="Assembly"/> to scan.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the actions to.</param>
	/// <param name="defaultLifetime">The default <see cref="ServiceLifetime"/> for actions to be registered with if one is not specified by a <see cref="RegisterActionAttribute.ServiceLifetime"/>.</param>
	/// <param name="mode">The <see cref="RegisterActionsMode"/> to use when searching for actions.</param>
	/// <returns><paramref name="services"/> to allow for chaining.</returns>
	public static IServiceCollection AddActions<T>(this IServiceCollection services, ServiceLifetime defaultLifetime, RegisterActionsMode mode) =>
		services.AddActions(defaultLifetime, mode, typeof(T).Assembly);

	/// <summary>
	///     Adds <see cref="IAction{TInput, TOutput}"/>s from the specified <paramref name="assemblies"/>.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the actions to.</param>
	/// <param name="defaultLifetime">The default <see cref="ServiceLifetime"/> for actions to be registered with if one is not specified by a <see cref="RegisterActionAttribute.ServiceLifetime"/>.</param>
	/// <param name="mode">The <see cref="RegisterActionsMode"/> to use when searching for actions.</param>
	/// <param name="assembly">The first <see cref="Assembly"/> to scan.</param>
	/// <param name="otherAssemblies">The other <see cref="Assembly"/>s to scan.</param>
	/// <returns><paramref name="services"/> to allow for chaining.</returns>
	public static IServiceCollection AddActions(this IServiceCollection services, ServiceLifetime defaultLifetime, RegisterActionsMode mode, Assembly assembly, params Assembly[] otherAssemblies) =>
		services.AddActions(otherAssemblies.Prepend(assembly), defaultLifetime, mode);

	/// <summary>
	///     Adds <see cref="IAction{TInput, TOutput}"/>s from the specified <paramref name="assemblies"/>.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the actions to.</param>
	/// <param name="assemblies">The <see cref="Assembly"/>s to scan.</param>
	/// <param name="defaultLifetime">The default <see cref="ServiceLifetime"/> for actions to be registered with if one is not specified by a <see cref="RegisterActionAttribute.ServiceLifetime"/>.</param>
	/// <param name="mode">The <see cref="RegisterActionsMode"/> to use when searching for actions.</param>
	/// <returns><paramref name="services"/> to allow for chaining.</returns>
	public static IServiceCollection AddActions(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime defaultLifetime = ServiceLifetime.Scoped, RegisterActionsMode mode = RegisterActionsMode.Implicit)
	{
		var actionTypes = TyneAssemblyActionScanner.GetActionTypes(assemblies, defaultLifetime, mode);
		foreach ((Type type, ServiceLifetime serviceLifetime) in actionTypes)
			services.Add(ServiceDescriptor.Describe(type, type, serviceLifetime));

		return services;
	}
}

