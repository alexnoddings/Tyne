using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Tyne.Docs.Services.Pages;

public static class PageService
{
	private static ILookup<PageCategory, PageInfo> CategoryPages { get; }

	static PageService()
	{
		CategoryPages =
			// Find all types in assembly
			(from type in Assembly.GetExecutingAssembly().GetTypes()
			 // Pages can't be abstract
			 where !type.IsAbstract
			 // Get the route attribute
			 let routeAttribute = type.GetCustomAttribute<RouteAttribute>()
			 where routeAttribute is not null
			 // Get the docs page attribute
			 let docsPageAttribute = type.GetCustomAttribute<DocsPageAttribute>()
			 where docsPageAttribute is not null
			 let category = docsPageAttribute.Category
			 let title = docsPageAttribute.Title
			 let route = routeAttribute.Template
			 // The route can't contain a parameter
			 where !route.Contains('{')
			 orderby title ascending
			 select (category, title, route))
			 .ToLookup(
				page => page.category, 
				page => new PageInfo(page.category, page.title, page.route)
			);

	}

	public static IEnumerable<PageInfo> GetPages(PageCategory category) =>
		CategoryPages[category];
}
