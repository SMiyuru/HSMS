using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HSMS.Data;
using HSMS.Models;
using HSMS.Services;

namespace HSMS.Controllers;

[Authorize]
public class SalesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;

    public SalesController(ApplicationDbContext context, ProductService productService, OrderService orderService)
    {
        _context = context;
        _productService = productService;
        _orderService = orderService;
    }

    [Authorize(Roles = "Admin,Staff")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Counter()
    {
        var products = await _productService.GetAllProductsAsync();
        return View(products);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    public async Task<IActionResult> CreateCounterOrder([FromBody] Order order)
    {
        if (order.OrderItems == null || !order.OrderItems.Any())
        {
            return Json(new { success = false, message = "No items in cart" });
        }

        order.OrderType = OrderType.Counter;
        order.StaffId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
        order.Status = OrderStatus.Confirmed;

        foreach (var item in order.OrderItems)
        {
            item.Product = await _context.Products.FindAsync(item.ProductId);
        }

        var createdOrder = await _orderService.CreateOrderAsync(order);
        
        return Json(new { success = true, orderId = createdOrder.Id, orderNumber = createdOrder.OrderNumber });
    }

    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetProductByBarcode(string code)
    {
        var product = await _productService.GetProductByCodeAsync(code);
        if (product == null)
            return Json(new { success = false });

        return Json(new 
        { 
            success = true, 
            product = new 
            { 
                id = product.Id,
                code = product.Code,
                name = product.Name,
                price = product.SellingPrice,
                stock = product.CurrentStock
            } 
        });
    }

    public async Task<IActionResult> Invoice(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return View(order);
    }

    public async Task<IActionResult> GetOrders(string search, string type)
    {
        List<Order> orders;
        
        if (!string.IsNullOrEmpty(search))
        {
            orders = await _orderService.GetAllOrdersAsync();
            orders = orders.Where(o => 
                o.OrderNumber.Contains(search) ||
                (o.Customer?.FullName.Contains(search) ?? false))
                .ToList();
        }
        else if (type == "today")
        {
            orders = await _orderService.GetTodayOrdersAsync();
        }
        else
        {
            orders = await _orderService.GetAllOrdersAsync();
        }

        return View(orders);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        foreach (var item in order.OrderItems)
        {
            await _productService.AdjustStockAsync(item.ProductId, item.Quantity);
        }

        await _orderService.UpdateOrderStatusAsync(id, OrderStatus.Cancelled);
        TempData["Success"] = "Order cancelled successfully!";
        
        return RedirectToAction(nameof(GetOrders));
    }
}