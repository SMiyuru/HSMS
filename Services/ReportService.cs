using Microsoft.EntityFrameworkCore;
using HSMS.Data;
using HSMS.Models;

namespace HSMS.Services;

public class ReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, decimal>> GetDailySalesReportAsync(DateTime date)
    {
        var orders = await _context.Orders
            .Where(o => o.CreatedAt.Date == date.Date && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        return new Dictionary<string, decimal>
        {
            ["TotalOrders"] = orders.Count,
            ["TotalRevenue"] = (decimal)orders.Sum(o => (double)o.FinalAmount),
            ["TotalDiscount"] = (decimal)orders.Sum(o => (double)o.Discount),
            ["TotalTax"] = (decimal)orders.Sum(o => (double)o.TaxAmount),
            ["CashPayments"] = (decimal)orders.Where(o => o.PaymentMethod == PaymentMethod.Cash).Sum(o => (double)o.FinalAmount),
            ["CardPayments"] = (decimal)orders.Where(o => o.PaymentMethod == PaymentMethod.Card).Sum(o => (double)o.FinalAmount),
            ["CODPayments"] = (decimal)orders.Where(o => o.PaymentMethod == PaymentMethod.COD).Sum(o => (double)o.FinalAmount)
        };
    }

    public async Task<Dictionary<string, object>> GetMonthlySalesReportAsync(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var orders = await _context.Orders
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        var dailySales = orders
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Total = (decimal)g.Sum(o => (double)o.FinalAmount) })
            .OrderBy(x => x.Date)
            .ToList();

        return new Dictionary<string, object>
        {
            ["StartDate"] = startDate,
            ["EndDate"] = endDate,
            ["TotalOrders"] = orders.Count,
            ["TotalRevenue"] = (decimal)orders.Sum(o => (double)o.FinalAmount),
            ["TotalDiscount"] = (decimal)orders.Sum(o => (double)o.Discount),
            ["AverageOrderValue"] = orders.Any() ? (decimal)orders.Average(o => (double)o.FinalAmount) : 0,
            ["DailySales"] = dailySales
        };
    }

    public async Task<List<Product>> GetStockReportAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderBy(p => p.CurrentStock)
            .ToListAsync();
    }

    public async Task<List<Product>> GetLowStockReportAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CurrentStock <= p.ReorderLevel && p.IsActive)
            .OrderBy(p => p.CurrentStock)
            .ToListAsync();
    }

    public async Task<List<BestSellingProductDto>> GetBestSellingProductsReportAsync(DateTime startDate, DateTime endDate)
    {
        var items = await _context.OrderItems
            .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
            .Where(oi => oi.Order != null && 
                       oi.Order.CreatedAt >= startDate && 
                       oi.Order.CreatedAt <= endDate &&
                       oi.Order.Status != OrderStatus.Cancelled)
            .ToListAsync();

        var results = items
            .GroupBy(oi => new { oi.ProductId, ProductName = oi.Product?.Name ?? "", CategoryName = oi.Product?.Category?.Name ?? "" })
            .Select(g => new BestSellingProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                CategoryName = g.Key.CategoryName,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = (decimal)g.Sum(x => (double)x.TotalPrice)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ToList();
        
        return results;
    }

    public async Task<Dictionary<string, decimal>> GetProfitReportAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        var totalRevenue = (decimal)orders.Sum(o => (double)o.FinalAmount);
        var totalCost = (decimal)orders.SelectMany(o => o.OrderItems)
            .Sum(oi => (double)((oi.Product?.PurchasePrice ?? 0) * oi.Quantity));
        var profit = totalRevenue - totalCost;

        return new Dictionary<string, decimal>
        {
            ["TotalRevenue"] = totalRevenue,
            ["TotalCost"] = totalCost,
            ["Profit"] = profit,
            ["ProfitMargin"] = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0
        };
    }
}