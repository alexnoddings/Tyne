namespace Tyne.Docs.Data;

public class Item
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public Guid LocationId { get; set; }
	public Location Location { get; set; } = default!;
}
