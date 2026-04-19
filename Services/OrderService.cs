using Microsoft.EntityFrameworkCore;
using HSMS.Data;
using HSMS.Models;

namespace HSMS.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ProductService _productService;

    public OrderService(ApplicationDbContext context, ProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Staff)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetTodayOrdersAsync()
    {
        var today = DateTime.Today;
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.CreatedAt.Date == today)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.CreatedAt.Date >= startDate.Date && o.CreatedAt.Date <= endDate.Date)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Staff)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        order.OrderNumber = GenerateOrderNumber();
        order.CreatedAt = DateTime.Now;

        foreach (var item in order.OrderItems)
        {
            item.UnitPrice = item.Product?.SellingPrice ?? 0;
            item.TotalPrice = item.UnitPrice * item.Quantity;

            if (order.OrderType == OrderType.Counter || 
                (order.PaymentMethod == PaymentMethod.Card && order.Status == OrderStatus.Confirmed))
            {
                await _productService.AdjustStockAsync(item.ProductId, -item.Quantity);
            }
        }

        order.TotalAmount = (decimal)order.OrderItems.Sum(oi => (double)oi.TotalPrice);
        order.FinalAmount = order.TotalAmount - order.Discount + order.TaxAmount;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            order.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> GetTodaySalesTotalAsync()
    {
        var today = DateTime.Today;
        var orders = await _context.Orders
            .Where(o => o.CreatedAt.Date == today && o.Status != OrderStatus.Cancelled)
            .ToListAsync();
        return (decimal)orders.Sum(o => (double)o.FinalAmount);
    }

    public async Task<int> GetTodayOrdersCountAsync()
    {
        var today = DateTime.Today;
        return await _context.Orders
            .Where(o => o.CreatedAt.Date == today)
            .CountAsync();
    }

    public async Task<List<Product>> GetTopSellingProductsAsync(int count = 5)
    {
        var startDate = DateTime.Today;
        var items = await _context.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => oi.Order != null && oi.Order.CreatedAt.Date == startDate)
            .ToListAsync();
        
        var grouped = items
            .GroupBy(oi => oi.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQty = g.Sum(x => x.Quantity) })
            .OrderByDescending(g => g.TotalQty)
            .Take(count)
            .ToList();

        var productIds = grouped.Select(g => g.ProductId).ToList();
        return await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
    }
}