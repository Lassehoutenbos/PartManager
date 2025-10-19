namespace PartManager.Api.Models;

public class Drawer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DrawerType Type { get; set; } = DrawerType.GridfinityDrawer;
    
    // Gridfinity position (origin at 1,1 bottom-left, X→right, Y→up)
    public int GridX { get; set; } = 1;
    public int GridY { get; set; } = 1;
    
    // Bin dimensions (can span multiple cells)
    public int GridWidth { get; set; } = 1;
    public int GridHeight { get; set; } = 1;
    
    public string? Description { get; set; }
    
    // NFC/QR code support
    public string? NfcTagId { get; set; }
    public string? QrCode { get; set; }
    
    public ICollection<Part> Parts { get; set; } = new List<Part>();
}

public enum DrawerType
{
    GridfinityDrawer,
    Shelf,
    Box,
    Cabinet,
    OffSite,
    Other
}
