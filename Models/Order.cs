namespace HSMS.Models;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
    public ApplicationUser? Customer { get; set; }
    public string? StaffId { get; set; }
    public ApplicationUser? Staff { get; set; }
    public OrderType OrderType { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Notes { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}