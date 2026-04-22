using HSMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HSMS.Services;

public class AuditService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(string action, string entityName, int? entityId = null, string changes = "")
    {
        var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System";
        
        string ipAddress = "Unknown";
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                if (ipAddress == "::1")
                    ipAddress = "127.0.0.1";
                
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    ipAddress = httpContext.Request.Headers["X-Forwarded-First"];
            }
        }
        catch
        {
            ipAddress = "Unknown";
        }

        var auditLog = new AuditLog
        {
            UserName = userName,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Changes = changes,
            IpAddress = ipAddress,
            CreatedAt = DateTime.Now
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetLogsAsync(int limit = 100)
    {
        return await _context.AuditLogs
            .OrderByDescending(a => a.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<AuditLog>> GetLogsByEntityAsync(string entityName, int entityId)
    {
        return await _context.AuditLogs
            .Where(a => a.EntityName == entityName && a.EntityId == entityId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AuditLog>> GetLogsByUserAsync(string userName)
    {
        return await _context.AuditLogs
            .Where(a => a.UserName == userName)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<AuditLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.AuditLogs
            .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}