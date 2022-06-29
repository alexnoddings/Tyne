using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Tyne.Actions;

public static class TyneAssemblyActionScanner
{
	public sealed record ActionType(Type Type, ServiceLifetime ServiceLifetime);

	public static List<ActionType> GetActionTypes(Assembly assembly, params Assembly[] otherAssemblies) =>
		GetActionTypes(ServiceLifetime.Scoped, RegisterActionsMode.Implicit, assembly, otherAssemblies);

	public static List<ActionType> GetActionTypes(ServiceLifetime defaultLifetime, RegisterActionsMode mode, Assembly assembly, params Assembly[] otherAssemblies) =>
		GetActionTypes(otherAssemblies.Prepend(assembly), defaultLifetime, mode);

	public static List<ActionType> GetActionTypes(IEnumerable<Assembly> assemblies, ServiceLifetime defaultLifetime, RegisterActionsMode mode)
	{
		if (!Enum.IsDefined(mode))
			throw new ArgumentOutOfRangeException(nameof(mode));

		return GetActionTypesCore(assemblies, defaultLifetime, mode).ToList();
	}

	private static IEnumerable<ActionType> GetActionTypesCore(IEnumerable<Assembly> assemblies, ServiceLifetime defaultLifetime, RegisterActionsMode mode)
	{
		Type openGenericActionType = typeof(IAction<,>);
		foreach (Assembly assembly in assemblies)
		{
			foreach (Type type in assembly.GetTypes())
			{
				// Don't register abstract or generic types
				if (type.IsAbstract || type.IsGenericTypeDefinition)
					continue;

				// Only register types which implement IAction<,>
				if (!type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGenericActionType))
					continue;

				RegisterActionAttribute? registerActionAttribute = type.GetCustomAttribute<RegisterActionAttribute>();

				// In implicit mode, only exclude actions with a RegisterActionAttribute and ShouldRegister set as false
				if (mode == RegisterActionsMode.Implicit && registerActionAttribute?.ShouldRegister == false)
					continue;

				// In explicit mode, only include actions with a RegisterActionAttribute and ShouldRegister set as true
				if (mode == RegisterActionsMode.Explicit && registerActionAttribute?.ShouldRegister != true)
					continue;

				yield return new(type, registerActionAttribute?.ServiceLifetime ?? defaultLifetime);
			}
		}
	}
}
