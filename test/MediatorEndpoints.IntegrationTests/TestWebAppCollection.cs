using System.Diagnostics.CodeAnalysis;

namespace Tyne.MediatorEndpoints;

[CollectionDefinition(Name)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is an xUnit convention.")]
public class TestWebAppCollection : ICollectionFixture<TestWebAppFactory>
{
    public const string Name = nameof(TestWebAppCollection);
}
