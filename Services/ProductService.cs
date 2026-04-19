using Microsoft.EntityFrameworkCore;
using HSMS.Data;
using HSMS.Models;

namespace HSMS.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<Product>> GetLowStockProductsAsync()
    {
        return await _context.Products
            .Where(p => p.CurrentStock <= p.ReorderLevel && p.IsActive)
            .OrderBy(p => p.CurrentStock)
            .ToListAsync();
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && (
                p.Name.Contains(searchTerm) ||
                p.Code.Contains(searchTerm) ||
                (p.Category != null && p.Category.Name.Contains(searchTerm))))
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetProductByCodeAsync(string code)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
    }

    public async Task<bool> AddProductAsync(Product product)
    {
        var existing = await GetProductByCodeAsync(product.Code);
        if (existing != null)
            return false;

        product.CreatedAt = DateTime.Now;
        product.UpdatedAt = DateTime.Now;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var existing = await _context.Products.FindAsync(product.Id);
        if (existing == null)
            return false;

        if (existing.Code != product.Code)
        {
            var duplicate = await GetProductByCodeAsync(product.Code);
            if (duplicate != null)
                return false;
        }

        existing.Name = product.Name;
        existing.Code = product.Code;
        existing.CategoryId = product.CategoryId;
        existing.PurchasePrice = product.PurchasePrice;
        existing.SellingPrice = product.SellingPrice;
        existing.CurrentStock = product.CurrentStock;
        existing.ReorderLevel = product.ReorderLevel;
        existing.ImageUrl = product.ImageUrl;
        existing.SupplierId = product.SupplierId;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        product.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task AdjustStockAsync(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product != null)
        {
            product.CurrentStock += quantity;
            product.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}