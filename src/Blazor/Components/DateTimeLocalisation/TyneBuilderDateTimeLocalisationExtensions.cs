using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Localisation;

/// <summary>
///     Extensions for registering <see cref="IUserTimeZoneService"/>s with <see cref="TyneBuilder"/>s.
/// </summary>
public static class TyneBuilderDateTimeLocalisationExtensions
{
    /// <summary>
    ///     Adds <see cref="JsUserTimeZoneService"/> as the <see cref="IUserTimeZoneService"/> implementation.
    /// </summary>
    /// <param name="tyneBuilder">The <see cref="TyneBuilder"/>.</param>
    /// <returns><paramref name="tyneBuilder"/> for chaining.</returns>
    /// <remarks>
    ///     See <see cref="JsUserTimeZoneService"/> for how it loads the user's <see cref="TimeZoneInfo"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBuilder"/> is <see langword="null"/>.</exception>
    public static TyneBuilder AddUserTimeZoneFromJavascript(this TyneBuilder tyneBuilder)
    {
        ArgumentNullException.ThrowIfNull(tyneBuilder);

        tyneBuilder.Services.AddScoped<IUserTimeZoneService, JsUserTimeZoneService>();
        return tyneBuilder;
    }
}
