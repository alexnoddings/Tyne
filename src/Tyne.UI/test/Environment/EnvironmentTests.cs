using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tyne.UI;

public class EnvironmentTests : TestContext
{
	public static object[][] ShouldShowData() => new[]
	{
		// Should show when env name is included
		new object[] { true, "Development", EnvironmentMode.Include, "Development" },
		new object[] { true, "Development", EnvironmentMode.Include, "Production, Development, Staging" },
		new object[] { true, "12345@!;", EnvironmentMode.Include, "12345@!;" },
		// Should not show when env name is not included
		new object[] { false, "Development", EnvironmentMode.Include, "Production" },
		new object[] { false, "Development", EnvironmentMode.Include, "Production, Staging" },
		new object[] { false, "Development", EnvironmentMode.Include, "12345@!;" },
		new object[] { false, "12345@!;", EnvironmentMode.Include, "Production" },
		
		// Should not show when env name is excluded
		new object[] { false, "Development", EnvironmentMode.Exclude, "Development" },
		// Should show when env name is not excluded
		new object[] { true, "Development", EnvironmentMode.Exclude, "Production" },

		// Should not show when set to include and env name is empty
		new object[] { false, "Development", EnvironmentMode.Include, "" },
		new object[] { false, "", EnvironmentMode.Include, "" },
		// Should show when set to exclude and env name is empty
		new object[] { true, "Development", EnvironmentMode.Exclude, "" },
		new object[] { true, "", EnvironmentMode.Exclude, "" },

		// Should not show when set to include and the env name is a substring
		new object[] { false, "Dev", EnvironmentMode.Include, "Development" },
		// Should show when set to exclude and the env name is a substring
		new object[] { true, "Dev", EnvironmentMode.Exclude, "Development" },

		// Should be case insensitive
		new object[] { true, "development", EnvironmentMode.Include, "DEVELOPMENT" },
		new object[] { false, "development", EnvironmentMode.Exclude, "DEVELOPMENT" },
	};

	[Theory]
	[MemberData(nameof(ShouldShowData))]
	public void ShouldShow(bool shouldShowContent, string hostEnvironmentName, EnvironmentMode mode, string filter)
	{
		// Arrange
		var hostEnvironmentMock = new Mock<IHostEnvironment>();
		hostEnvironmentMock.Setup(hostEnvironment => hostEnvironment.EnvironmentName).Returns(hostEnvironmentName);
		Services.AddSingleton(hostEnvironmentMock.Object);

		const string environmentContentText = "Environment Unit Test Render Content";
		var wasRenderFragmentExecuted = false;
		RenderFragment childContent(string _)
		{
			wasRenderFragmentExecuted = true;
			return builder => builder.AddContent(0, environmentContentText);
		}

		// Act
		var cut = RenderComponent<Environment>(parameters => parameters
			.Add(environment => environment.Mode, mode)
			.Add(environment => environment.Filter, filter)
			.Add(environment => environment.ChildContent, childContent)
		);

		// Assert
		if (shouldShowContent)
		{
			// The render fragment should have been executed
			Assert.True(wasRenderFragmentExecuted);
			// And the markup should contain our test string
			Assert.Contains(environmentContentText, cut.Markup);
		}
		else
		{
			// The render fragment should not have been executed
			Assert.False(wasRenderFragmentExecuted);
			// And the markup should not contain our test string
			Assert.DoesNotContain(environmentContentText, cut.Markup);
		}
	}

	public static object[][] ShouldRenderEnvironmentNameData() => new[]
	{
		new object[] { "Development" },
		new object[] { "dEvelOpmEnT" },
		new object[] { "Production" },
		new object[] { "abc xyz" },
		new object[] { "12345@!;" },
	};

	[Theory]
	[MemberData(nameof(ShouldRenderEnvironmentNameData))]
	public void ShouldRenderEnvironmentName(string hostEnvironmentName)
	{
		// Arrange
		var hostEnvironmentMock = new Mock<IHostEnvironment>();
		hostEnvironmentMock.Setup(hostEnvironment => hostEnvironment.EnvironmentName).Returns(hostEnvironmentName);
		Services.AddSingleton(hostEnvironmentMock.Object);

		var wasRenderFragmentExecuted = false;
		RenderFragment childContent(string _)
		{
			wasRenderFragmentExecuted = true;
			return builder => builder.AddContent(0, hostEnvironmentName);
		}

		// Act
		var cut = RenderComponent<Environment>(parameters => parameters
			.Add(environment => environment.Mode, EnvironmentMode.Include)
			.Add(environment => environment.Filter, hostEnvironmentName)
			.Add(environment => environment.ChildContent, childContent)
		);

		// Assert
		Assert.True(wasRenderFragmentExecuted);
		Assert.Contains(hostEnvironmentName, cut.Markup);
	}
}
