namespace PartManager.Api.Models;

public class Drawer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int GridRow { get; set; }
    public int GridColumn { get; set; }
    public string? Description { get; set; }
    public ICollection<Part> Parts { get; set; } = new List<Part>();
}
