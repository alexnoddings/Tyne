using Microsoft.Extensions.Configuration;
using Tyne.UI;

namespace Microsoft.Extensions.DependencyInjection;

public static class TitleServiceCollectionExtensions
{
	public static IServiceCollection AddTyneTitle(this IServiceCollection services)
	{
		if (services is null)
			throw new ArgumentNullException(nameof(services));

		services
			.AddOptions<TitleOptions>()
			.Configure<IConfiguration>((options, configuration) => configuration.Bind(TitleOptions.ConfigurationSectionName, options));

		return services;
	}

	public static IServiceCollection AddTyneTitle(this IServiceCollection services, IConfiguration configuration)
	{
		if (services is null)
			throw new ArgumentNullException(nameof(services));

		if (configuration is null)
			throw new ArgumentNullException(nameof(configuration));

		services
			.AddOptions<TitleOptions>()
			.Configure(options => configuration.Bind(TitleOptions.ConfigurationSectionName, options));

		return services;
	}

	public static IServiceCollection AddTyneTitle(this IServiceCollection services, string appName, string divider = " | ", bool isSuffix = true)
	{
		if (services is null)
			throw new ArgumentNullException(nameof(services));

		appName ??= string.Empty;
		divider ??= string.Empty;

		services
			.AddOptions<TitleOptions>()
			.Configure(options =>
			{
				options.AppName = appName;
				options.Divider = divider;
				options.IsSuffix = isSuffix;
			});

		return services;
	}

	public static IServiceCollection AddTyneTitle(this IServiceCollection services, Action<TitleOptions> configure)
	{
		if (services is null)
			throw new ArgumentNullException(nameof(services));

		if (configure is null)
			throw new ArgumentNullException(nameof(configure));

		services
			.AddOptions<TitleOptions>()
			.Configure(configure);

		return services;
	}
}
