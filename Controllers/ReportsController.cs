using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HSMS.Services;

namespace HSMS.Controllers;

[Authorize(Roles = "Admin")]
public class ReportsController : Controller
{
    private readonly ReportService _reportService;
    private readonly OrderService _orderService;

    public ReportsController(ReportService reportService, OrderService orderService)
    {
        _reportService = reportService;
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Sales(string type, DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.Today;
        var end = endDate ?? DateTime.Today;

        switch (type)
        {
            case "daily":
                var daily = await _reportService.GetDailySalesReportAsync(start);
                ViewBag.Report = daily;
                ViewBag.Title = $"Daily Sales Report - {start:yyyy-MM-dd}";
                break;
            case "monthly":
                var monthly = await _reportService.GetMonthlySalesReportAsync(start.Year, start.Month);
                ViewBag.Report = monthly;
                ViewBag.Title = $"Monthly Sales Report - {start:MMMM yyyy}";
                break;
            case "custom":
                var orders = await _orderService.GetOrdersByDateRangeAsync(start, end);
                ViewBag.Orders = orders;
                ViewBag.StartDate = start;
                ViewBag.EndDate = end;
                ViewBag.Title = $"Sales Report - {start:yyyy-MM-dd} to {end:yyyy-MM-dd}";
                break;
            default:
                var todayReport = await _reportService.GetDailySalesReportAsync(DateTime.Today);
                ViewBag.Report = todayReport;
                ViewBag.Title = $"Today's Sales Report - {DateTime.Today:yyyy-MM-dd}";
                break;
        }

        return View();
    }

    public async Task<IActionResult> Stock()
    {
        var products = await _reportService.GetStockReportAsync();
        return View(products);
    }

    public async Task<IActionResult> LowStock()
    {
        var products = await _reportService.GetLowStockReportAsync();
        return View(products);
    }

    public async Task<IActionResult> BestSelling(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.Today.AddDays(-30);
        var end = endDate ?? DateTime.Today;

        var products = await _reportService.GetBestSellingProductsReportAsync(start, end);
        ViewBag.StartDate = start;
        ViewBag.EndDate = end;
        return View(products);
    }

    public async Task<IActionResult> Profit(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.Today.AddMonths(-1);
        var end = endDate ?? DateTime.Today;

        var profit = await _reportService.GetProfitReportAsync(start, end);
        ViewBag.Report = profit;
        ViewBag.StartDate = start;
        ViewBag.EndDate = end;
        return View(profit);
    }

    public async Task<IActionResult> ExportToPdf(string type, DateTime? startDate, DateTime? endDate)
    {
        return RedirectToAction(type, new { startDate, endDate });
    }

    public async Task<IActionResult> ExportToExcel(string type, DateTime? startDate, DateTime? endDate)
    {
        return RedirectToAction(type, new { startDate, endDate });
    }
}