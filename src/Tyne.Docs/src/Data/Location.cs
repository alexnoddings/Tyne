namespace Tyne.Docs.Data;

public class Location
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public List<Item> Items { get; set; } = new();
}
