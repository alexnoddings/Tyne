using System.Reflection;

namespace Tyne.Blazor;

/// <summary>
///     The non-generic version of <see cref="ITyneTablePersistedFilter{TValue}"/>
///     which exposes <see cref="PersistKey"/> without needing a generic parameter.
/// </summary>
public interface ITyneTablePersistedFilter
{
    /// <summary>
    ///     A key to persist <see cref="ITyneTableValueFilter{TValue}.Value"/> as.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If <see cref="TyneTableKey.IsEmpty"/>, persistence will be disabled.
    ///     </para>
    ///     <para>
    ///         For columns which implement a <c>For="..."</c> property,
    ///         <see cref="TyneTableKey.From(string?, PropertyInfo?)"/> should be used.
    ///         Otherwise, <see cref="TyneTableKey.From(string?)"/> should be used if
    ///         there are no properties known about.
    ///     </para>
    /// </remarks>
    public TyneTableKey PersistKey { get; }
}
