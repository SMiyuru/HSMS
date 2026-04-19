namespace HSMS.Models;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}

public enum PaymentMethod
{
    Cash = 0,
    Card = 1,
    COD = 2
}

public enum OrderType
{
    Counter = 0,
    Online = 1
}