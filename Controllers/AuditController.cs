using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HSMS.Services;
using HSMS.Models;

namespace HSMS.Controllers;

[Authorize(Roles = "Admin")]
public class AuditController : Controller
{
    private readonly AuditService _auditService;

    public AuditController(AuditService auditService)
    {
        _auditService = auditService;
    }

    public async Task<IActionResult> Index(string search, DateTime? startDate, DateTime? endDate, int page = 1)
    {
        const int pageSize = 20;
        
        List<AuditLog> allLogs;
        
        if (startDate.HasValue && endDate.HasValue)
        {
            allLogs = await _auditService.GetLogsByDateRangeAsync(startDate.Value, endDate.Value);
        }
        else if (!string.IsNullOrEmpty(search))
        {
            var logs = await _auditService.GetLogsAsync(1000);
            allLogs = logs.Where(l => 
                l.UserName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                l.Action.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                l.EntityName.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
        else
        {
            allLogs = await _auditService.GetLogsAsync(1000);
        }

        var totalLogs = allLogs.Count;
        var totalPages = (int)Math.Ceiling(totalLogs / (double)pageSize);
        
        var logsToShow = allLogs
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.Search = search;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalLogs = totalLogs;
        
        return View(logsToShow);
    }
}