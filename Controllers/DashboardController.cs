using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HSMS.Services;
using HSMS.Models;

namespace HSMS.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly OrderService _orderService;
    private readonly ProductService _productService;
    private readonly ApplicationDbContext _context;

    public DashboardController(OrderService orderService, ProductService productService, ApplicationDbContext context)
    {
        _orderService = orderService;
        _productService = productService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var todaySales = await _orderService.GetTodaySalesTotalAsync();
        var todayOrders = await _orderService.GetTodayOrdersCountAsync();
        var lowStockProducts = await _productService.GetLowStockProductsAsync();
        var topProducts = await _orderService.GetTopSellingProductsAsync(5);

        var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i)).ToList();
        var salesData = new List<double>();
        var orderCount = new List<int>();
        
        foreach (var date in last7Days)
        {
            var orders = _context.Orders.Where(o => o.CreatedAt.Date == date.Date && o.Status != OrderStatus.Cancelled).ToList();
            salesData.Add((double)orders.Sum(o => o.FinalAmount));
            orderCount.Add(orders.Count);
        }

        var categoryItems = _context.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p != null ? p.Category : null)
            .Where(oi => oi.Order != null && oi.Order.CreatedAt >= DateTime.Today.AddDays(-30))
            .ToList();

        var categorySales = categoryItems
            .GroupBy(oi => oi.Product?.Category?.Name ?? "Uncategorized")
            .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .ToList();

        var categoryLabels = categorySales.Select(c => c.Category).ToList();
        var categoryData = categorySales.Select(c => (double)c.Total).ToList();

        ViewBag.TodaySales = todaySales;
        ViewBag.TodayOrders = todayOrders;
        ViewBag.LowStockCount = lowStockProducts.Count;
        ViewBag.LowStockProducts = lowStockProducts;
        ViewBag.TopProducts = topProducts;
        
        ViewBag.SalesLabels = last7Days.Select(d => d.ToString("MM/dd")).ToList();
        ViewBag.SalesData = salesData;
        ViewBag.OrderCount = orderCount;
        
        ViewBag.CategoryLabels = categoryLabels;
        ViewBag.CategoryData = categoryData;

        return View();
    }
}