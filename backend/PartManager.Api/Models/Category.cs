namespace PartManager.Api.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Part> Parts { get; set; } = new List<Part>();
}
