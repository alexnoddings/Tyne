using Microsoft.Extensions.Configuration;
using Tyne.UI;

namespace Microsoft.Extensions.DependencyInjection;

public static class TitleServiceCollectionExtensions
{
	public static IServiceCollection AddTyneTitle(this IServiceCollection services, IConfiguration configuration) =>
		AddTyneTitleCore(services ,options => configuration.Bind(TitleOptions.ConfigurationSectionName, options));

	public static IServiceCollection AddTyneTitle(this IServiceCollection services, string appName, string divider = " | ", bool isSuffix = true) =>
		AddTyneTitleCore(services, options => {
			options.AppName = appName;
			options.Divider = divider;
			options.IsSuffix = isSuffix;
		});

	public static IServiceCollection AddTyneTitle(this IServiceCollection services, Action<TitleOptions> configure) =>
		AddTyneTitleCore(services, configure);

	private static IServiceCollection AddTyneTitleCore(IServiceCollection services, Action<TitleOptions> configure) 
	{
		if (services is null)
			throw new ArgumentNullException(nameof(services));

		if (configure is null)
			throw new ArgumentNullException(nameof(configure));

		return services.Configure<TitleOptions>(configure);
	}
}
