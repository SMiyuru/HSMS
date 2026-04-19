namespace HSMS.Models;

public class Purchase
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public List<PurchaseItem> PurchaseItems { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}