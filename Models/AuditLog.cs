namespace HSMS.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Action { get; set; } = "";
    public string EntityName { get; set; } = "";
    public int? EntityId { get; set; }
    public string Changes { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string IpAddress { get; set; } = "";
}

public enum AuditAction
{
    Create,
    Update,
    Delete,
    View,
    Login,
    Logout,
    Sale,
    StockAdjustment
}