using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using HSMS.Data;
using HSMS.Models;
using HSMS.Services;

namespace HSMS.Controllers;

[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ProductService _productService;
    private readonly IWebHostEnvironment _env;
    private readonly AuditService _auditService;

    public ProductsController(ApplicationDbContext context, ProductService productService, IWebHostEnvironment env, AuditService auditService)
    {
        _context = context;
        _productService = productService;
        _env = env;
        _auditService = auditService;
    }

    public async Task<IActionResult> Index(string search)
    {
        var products = string.IsNullOrEmpty(search)
            ? await _productService.GetAllProductsAsync()
            : await _productService.SearchProductsAsync(search);

        await _auditService.LogAsync("View", "Products", null, $"Search: {search}");

        ViewBag.Search = search;
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    public IActionResult Create()
    {
        ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _context.Categories.Where(c => c.IsActive), "Id", "Name");

        ViewData["SupplierId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _context.Suppliers.Where(s => s.IsActive), "Id", "Name");

        return PartialView("_CreateModal", new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] Product product, [FromForm] IFormFile? productImage)
    {
        if (ModelState.IsValid)
        {
            if (productImage != null && productImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(productImage.FileName)}";
                var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productImage.CopyToAsync(stream);
                }

                product.ImageUrl = $"/uploads/{fileName}";
            }

            var result = await _productService.AddProductAsync(product);

            if (result)
            {
                TempData["Success"] = "Product added successfully!";
                return Json(new { success = true });
            }

            ModelState.AddModelError("Code", "Product code already exists");
        }

        ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _context.Categories.Where(c => c.IsActive), "Id", "Name", product.CategoryId);

        ViewData["SupplierId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            _context.Suppliers.Where(s => s.IsActive), "Id", "Name", product.SupplierId);

        return PartialView("_CreateModal", product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories.Where(c => c.IsActive), "Id", "Name", product.CategoryId);
        ViewData["SupplierId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Suppliers.Where(s => s.IsActive), "Id", "Name", product.SupplierId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProductAsync(product);
            if (result)
            {
                await _auditService.LogAsync("Update", "Product", product.Id, $"Updated: {product.Name}");
                TempData["Success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("Code", "Product code already exists");
        }

        ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Categories.Where(c => c.IsActive), "Id", "Name", product.CategoryId);
        ViewData["SupplierId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Suppliers.Where(s => s.IsActive), "Id", "Name", product.SupplierId);
        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        await _productService.DeleteProductAsync(id);
        await _auditService.LogAsync("Delete", "Product", id, $"Deleted: {product?.Name} - {product?.Code}");
        TempData["Success"] = "Product deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult LowStock()
    {
        return RedirectToAction("Index", "Reports", new { type = "lowstock" });
    }
}