using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Actions;

/// <summary>
///     Controls an <see cref="IAction{TModel, TResult}"/>'s registration.
/// </summary>
/// <remarks>
///     Disable an action from being registered with <see cref="RegisterActionAttribute(bool)"/>, or override the default service lifetime with <see cref="RegisterActionAttribute(ServiceLifetime)"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class RegisterActionAttribute : Attribute
{
    /// <summary>
    ///     If the action should be registered. See <see cref="RegisterActionsMode"/> for more info.
    /// </summary>
    public bool ShouldRegister { get; }

    /// <summary>
    ///     The <see cref="ServiceLifetime"/> this action should use. Will use the default when <see langword="null"/>.
    /// </summary>
    public ServiceLifetime? ServiceLifetime { get; }

    public RegisterActionAttribute(bool shouldRegister = true) : this(shouldRegister, null)
    {
    }

    public RegisterActionAttribute(ServiceLifetime serviceLifetime) : this(true, serviceLifetime)
    {
    }

    private RegisterActionAttribute(bool shouldRegister, ServiceLifetime? serviceLifetime)
    {
        ShouldRegister = shouldRegister;
        ServiceLifetime = serviceLifetime;
    }
}
