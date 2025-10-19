namespace PartManager.Api.Models;

public class Part
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PartNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? Datasheet { get; set; }
    public int Quantity { get; set; }
    public int? DrawerId { get; set; }
    public Drawer? Drawer { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
