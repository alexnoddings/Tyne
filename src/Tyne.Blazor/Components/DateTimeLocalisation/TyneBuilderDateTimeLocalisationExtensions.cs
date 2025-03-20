using Microsoft.Extensions.DependencyInjection;

namespace Tyne.Blazor.Localisation;

/// <summary>
///     Extensions for registering <see cref="IUserTimeZoneService"/>s with <see cref="TyneBlazorBuilder"/>s.
/// </summary>
public static class TyneBlazorBuilderDateTimeLocalisationExtensions
{
    /// <summary>
    ///     Adds <see cref="JsUserTimeZoneService"/> as the <see cref="IUserTimeZoneService"/> implementation.
    /// </summary>
    /// <param name="tyneBlazorBuilder">The <see cref="TyneBlazorBuilder"/>.</param>
    /// <returns><paramref name="tyneBlazorBuilder"/> for chaining.</returns>
    /// <remarks>
    ///     See <see cref="JsUserTimeZoneService"/> for how it loads the user's <see cref="TimeZoneInfo"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="tyneBlazorBuilder"/> is <see langword="null"/>.</exception>
    public static TyneBlazorBuilder AddUserTimeZoneFromJavascript(this TyneBlazorBuilder tyneBlazorBuilder)
    {
        ArgumentNullException.ThrowIfNull(tyneBlazorBuilder);

        _ = tyneBlazorBuilder.Services.AddScoped<IUserTimeZoneService, JsUserTimeZoneService>();
        return tyneBlazorBuilder;
    }
}
