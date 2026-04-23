using Microsoft.AspNetCore.Identity;
using HSMS.Models;

namespace HSMS.Data;

public static class SeedData
{
    public static async Task InitializeAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (!context.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Staff"));
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Power Tools", Description = "Electric and battery-powered tools" },
                new Category { Name = "Hand Tools", Description = "Manual hand tools" },
                new Category { Name = "Fasteners", Description = "Nails, screws, bolts, nuts" },
                new Category { Name = "Electrical", Description = "Electrical supplies" },
                new Category { Name = "Plumbing", Description = "Plumbing supplies" },
                new Category { Name = "Paint", Description = "Paints and coatings" },
                new Category { Name = "Safety", Description = "Safety equipment" },
                new Category { Name = "Hardware", Description = "General hardware items" }
            );
            await context.SaveChangesAsync();
        }

        if (!context.Suppliers.Any())
        {
            context.Suppliers.AddRange(
                new Supplier { Name = "Colombo Hardware Supply", ContactPerson = "John Fernando", Email = "john@colombohardware.lk", Phone = "0112345678", Address = "Colombo 10" },
                new Supplier { Name = "Lanka Tools Distributors", ContactPerson = "Mary Perera", Email = "mary@lankatools.lk", Phone = "0112345679", Address = "Colombo 08" },
                new Supplier { Name = "Metro Electricals", ContactPerson = "Sam Silva", Email = "sam@metroelectricals.lk", Phone = "0112345680", Address = "Kandy" },
                new Supplier { Name = "Premier Plumbing", ContactPerson = "Kumarage", Email = "kumar@premierplumb.lk", Phone = "0112345681", Address = "Galle" },
                new Supplier { Name = "Paint World", ContactPerson = "Nimal", Email = "nimal@paintworld.lk", Phone = "0112345682", Address = "Colombo 05" }
            );
            await context.SaveChangesAsync();
        }

        var catPowerTools = context.Categories.FirstOrDefault(c => c.Name == "Power Tools")?.Id ?? 1;
        var catHandTools = context.Categories.FirstOrDefault(c => c.Name == "Hand Tools")?.Id ?? 2;
        var catFasteners = context.Categories.FirstOrDefault(c => c.Name == "Fasteners")?.Id ?? 3;
        var catElectrical = context.Categories.FirstOrDefault(c => c.Name == "Electrical")?.Id ?? 4;
        var catPlumbing = context.Categories.FirstOrDefault(c => c.Name == "Plumbing")?.Id ?? 5;
        var catPaint = context.Categories.FirstOrDefault(c => c.Name == "Paint")?.Id ?? 6;
        var catSafety = context.Categories.FirstOrDefault(c => c.Name == "Safety")?.Id ?? 7;
        var catHardware = context.Categories.FirstOrDefault(c => c.Name == "Hardware")?.Id ?? 8;

        if (!context.Products.Any())
        {
            var products = new List<Product>();
            
            // Power Tools
            products.AddRange(new[] {
                new Product { Code = "PT001", Name = "Cordless Drill 18V", CategoryId = catPowerTools, PurchasePrice = 8500, SellingPrice = 12500, CurrentStock = 25, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "PT002", Name = "Cordless Drill 12V", CategoryId = catPowerTools, PurchasePrice = 6500, SellingPrice = 9500, CurrentStock = 30, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "PT003", Name = "Impact Driver 18V", CategoryId = catPowerTools, PurchasePrice = 9200, SellingPrice = 13500, CurrentStock = 18, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "PT004", Name = "Circular Saw 7.25 inch", CategoryId = catPowerTools, PurchasePrice = 7500, SellingPrice = 11000, CurrentStock = 15, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "PT005", Name = "Angle Grinder 4.5 inch", CategoryId = catPowerTools, PurchasePrice = 4500, SellingPrice = 6800, CurrentStock = 40, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "PT006", Name = "Angle Grinder 5 inch", CategoryId = catPowerTools, PurchasePrice = 5200, SellingPrice = 7800, CurrentStock = 35, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "PT007", Name = "Orbital Sander", CategoryId = catPowerTools, PurchasePrice = 3800, SellingPrice = 5500, CurrentStock = 22, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "PT008", Name = "Jigsaw", CategoryId = catPowerTools, PurchasePrice = 5500, SellingPrice = 8200, CurrentStock = 12, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "PT009", Name = "Rotary Hammer Drill", CategoryId = catPowerTools, PurchasePrice = 12500, SellingPrice = 18500, CurrentStock = 8, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "PT010", Name = "Heat Gun 2000W", CategoryId = catPowerTools, PurchasePrice = 3500, SellingPrice = 5200, CurrentStock = 20, ReorderLevel = 20, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Hand Tools
            products.AddRange(new[] {
                new Product { Code = "HT001", Name = "Claw Hammer 16oz", CategoryId = catHandTools, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 120, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "HT002", Name = "Claw Hammer 24oz", CategoryId = catHandTools, PurchasePrice = 950, SellingPrice = 1400, CurrentStock = 85, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "HT003", Name = "Ball Pein Hammer 8oz", CategoryId = catHandTools, PurchasePrice = 750, SellingPrice = 1100, CurrentStock = 60, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "HT004", Name = "Rubber Mallet 16oz", CategoryId = catHandTools, PurchasePrice = 650, SellingPrice = 950, CurrentStock = 75, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "HT005", Name = "Screwdriver Set 6pcs", CategoryId = catHandTools, PurchasePrice = 1200, SellingPrice = 1800, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "HT006", Name = "Phillips Screwdriver Set", CategoryId = catHandTools, PurchasePrice = 800, SellingPrice = 1200, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "HT007", Name = "Flathead Screwdriver Set", CategoryId = catHandTools, PurchasePrice = 800, SellingPrice = 1200, CurrentStock = 75, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "HT008", Name = "Adjustable Wrench 10 inch", CategoryId = catHandTools, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 90, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "HT009", Name = "Adjustable Wrench 12 inch", CategoryId = catHandTools, PurchasePrice = 1100, SellingPrice = 1600, CurrentStock = 65, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "HT010", Name = "Combination Pliers 8 inch", CategoryId = catHandTools, PurchasePrice = 750, SellingPrice = 1100, CurrentStock = 110, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "HT011", Name = "Long Nose Pliers 6 inch", CategoryId = catHandTools, PurchasePrice = 650, SellingPrice = 950, CurrentStock = 70, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "HT012", Name = "Diagonal Cutters 6 inch", CategoryId = catHandTools, PurchasePrice = 700, SellingPrice = 1000, CurrentStock = 55, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "HT013", Name = "Slip Joint Pliers 10 inch", CategoryId = catHandTools, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 60, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "HT014", Name = "Box End Wrench Set 10pcs", CategoryId = catHandTools, PurchasePrice = 2500, SellingPrice = 3800, CurrentStock = 35, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "HT015", Name = "Socket Set 52pcs", CategoryId = catHandTools, PurchasePrice = 4500, SellingPrice = 6800, CurrentStock = 45, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "HT016", Name = "Tape Measure 25ft", CategoryId = catHandTools, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 200, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "HT017", Name = "Tape Measure 50ft", CategoryId = catHandTools, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "HT018", Name = "Spirit Level 24 inch", CategoryId = catHandTools, PurchasePrice = 1200, SellingPrice = 1800, CurrentStock = 50, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "HT019", Name = "Spirit Level 48 inch", CategoryId = catHandTools, PurchasePrice = 1800, SellingPrice = 2600, CurrentStock = 30, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "HT020", Name = "Hacksaw", CategoryId = catHandTools, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 100, ReorderLevel = 30, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Fasteners
            products.AddRange(new[] {
                new Product { Code = "FS001", Name = "Wood Screws Assorted 500g", CategoryId = catFasteners, PurchasePrice = 350, SellingPrice = 500, CurrentStock = 300, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "FS002", Name = "Machine Screws Set 200g", CategoryId = catFasteners, PurchasePrice = 280, SellingPrice = 400, CurrentStock = 250, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "FS003", Name = "Nails 2 inch 1kg", CategoryId = catFasteners, PurchasePrice = 250, SellingPrice = 350, CurrentStock = 500, ReorderLevel = 150, SupplierId = 1 },
                new Product { Code = "FS004", Name = "Nails 3 inch 1kg", CategoryId = catFasteners, PurchasePrice = 280, SellingPrice = 400, CurrentStock = 400, ReorderLevel = 120, SupplierId = 1 },
                new Product { Code = "FS005", Name = "Bolts M6 x 50mm 100pc", CategoryId = catFasteners, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "FS006", Name = "Bolts M8 x 50mm 100pc", CategoryId = catFasteners, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 150, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "FS007", Name = "Nuts M6 500pc", CategoryId = catFasteners, PurchasePrice = 350, SellingPrice = 500, CurrentStock = 180, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "FS008", Name = "Nuts M8 500pc", CategoryId = catFasteners, PurchasePrice = 400, SellingPrice = 600, CurrentStock = 150, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "FS009", Name = "Washers M6 500pc", CategoryId = catFasteners, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "FS010", Name = "Washers M8 500pc", CategoryId = catFasteners, PurchasePrice = 300, SellingPrice = 450, CurrentStock = 180, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "FS011", Name = "Wall Anchors 100pc", CategoryId = catFasteners, PurchasePrice = 200, SellingPrice = 300, CurrentStock = 400, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "FS012", Name = "Drywall Screws 1kg", CategoryId = catFasteners, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 250, ReorderLevel = 80, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Electrical
            products.AddRange(new[] {
                new Product { Code = "EL001", Name = "Copper Wire 1.5sqmm", CategoryId = catElectrical, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 150, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "EL002", Name = "Copper Wire 2.5sqmm", CategoryId = catElectrical, PurchasePrice = 1200, SellingPrice = 1750, CurrentStock = 120, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "EL003", Name = "Copper Wire 4sqmm", CategoryId = catElectrical, PurchasePrice = 1850, SellingPrice = 2700, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "EL004", Name = "LED Bulb 9W B22", CategoryId = catElectrical, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 300, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "EL005", Name = "LED Bulb 12W B22", CategoryId = catElectrical, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 250, ReorderLevel = 70, SupplierId = 1 },
                new Product { Code = "EL006", Name = "CFL Bulb 20W B22", CategoryId = catElectrical, PurchasePrice = 350, SellingPrice = 500, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "EL007", Name = "Light Switch Single", CategoryId = catElectrical, PurchasePrice = 150, SellingPrice = 220, CurrentStock = 500, ReorderLevel = 150, SupplierId = 1 },
                new Product { Code = "EL008", Name = "Light Switch Double", CategoryId = catElectrical, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 400, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "EL009", Name = "Power Socket 13A", CategoryId = catElectrical, PurchasePrice = 280, SellingPrice = 420, CurrentStock = 350, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "EL010", Name = "Power Socket Flat", CategoryId = catElectrical, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 300, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "EL011", Name = "Circuit Breaker 32A", CategoryId = catElectrical, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "EL012", Name = "Circuit Breaker 63A", CategoryId = catElectrical, PurchasePrice = 1200, SellingPrice = 1750, CurrentStock = 50, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "EL013", Name = "Junction Box 2Way", CategoryId = catElectrical, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "EL014", Name = "Junction Box 4Way", CategoryId = catElectrical, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "EL015", Name = "Conduit Pipe 20mm 3m", CategoryId = catElectrical, PurchasePrice = 350, SellingPrice = 520, CurrentStock = 250, ReorderLevel = 80, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Plumbing
            products.AddRange(new[] {
                new Product { Code = "PL001", Name = "PVC Pipe 20mm 3m", CategoryId = catPlumbing, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "PL002", Name = "PVC Pipe 25mm 3m", CategoryId = catPlumbing, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 180, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "PL003", Name = "PVC Elbow 20mm", CategoryId = catPlumbing, PurchasePrice = 80, SellingPrice = 120, CurrentStock = 500, ReorderLevel = 150, SupplierId = 1 },
                new Product { Code = "PL004", Name = "PVC Elbow 25mm", CategoryId = catPlumbing, PurchasePrice = 100, SellingPrice = 150, CurrentStock = 400, ReorderLevel = 120, SupplierId = 1 },
                new Product { Code = "PL005", Name = "PVC Tee 20mm", CategoryId = catPlumbing, PurchasePrice = 100, SellingPrice = 150, CurrentStock = 350, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "PL006", Name = "PVC Tee 25mm", CategoryId = catPlumbing, PurchasePrice = 120, SellingPrice = 180, CurrentStock = 300, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "PL007", Name = "Brass Ball Valve 20mm", CategoryId = catPlumbing, PurchasePrice = 650, SellingPrice = 950, CurrentStock = 120, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "PL008", Name = "Brass Ball Valve 25mm", CategoryId = catPlumbing, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 90, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "PL009", Name = "Kitchen Mixer Tap", CategoryId = catPlumbing, PurchasePrice = 3500, SellingPrice = 5200, CurrentStock = 25, ReorderLevel = 8, SupplierId = 1 },
                new Product { Code = "PL010", Name = "Basin Mixer Tap", CategoryId = catPlumbing, PurchasePrice = 2800, SellingPrice = 4200, CurrentStock = 30, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "PL011", Name = "Shower Head", CategoryId = catPlumbing, PurchasePrice = 1200, SellingPrice = 1750, CurrentStock = 60, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "PL012", Name = "Shower Hose 1.5m", CategoryId = catPlumbing, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "PL013", Name = "P trap 40mm", CategoryId = catPlumbing, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 100, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "PL014", Name = "Flexi Connector 30cm", CategoryId = catPlumbing, PurchasePrice = 350, SellingPrice = 520, CurrentStock = 150, ReorderLevel = 50, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Paint
            products.AddRange(new[] {
                new Product { Code = "PA001", Name = "Wall Putty 5kg", CategoryId = catPaint, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 120, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "PA002", Name = "Primer 1L", CategoryId = catPaint, PurchasePrice = 650, SellingPrice = 950, CurrentStock = 150, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "PA003", Name = "Emulsion Paint White 1L", CategoryId = catPaint, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "PA004", Name = "Emulsion Paint White 5L", CategoryId = catPaint, PurchasePrice = 2200, SellingPrice = 3200, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "PA005", Name = "Enamel Paint 1L Red", CategoryId = catPaint, PurchasePrice = 750, SellingPrice = 1100, CurrentStock = 100, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "PA006", Name = "Enamel Paint 1L Blue", CategoryId = catPaint, PurchasePrice = 750, SellingPrice = 1100, CurrentStock = 90, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "PA007", Name = "Enamel Paint 1L Black", CategoryId = catPaint, PurchasePrice = 700, SellingPrice = 1000, CurrentStock = 85, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "PA008", Name = "Wood Polish 1L", CategoryId = catPaint, PurchasePrice = 1200, SellingPrice = 1750, CurrentStock = 50, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "PA009", Name = "Varnish 1L", CategoryId = catPaint, PurchasePrice = 950, SellingPrice = 1400, CurrentStock = 60, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "PA010", Name = "Paint Brush 1 inch", CategoryId = catPaint, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 250, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "PA011", Name = "Paint Brush 2 inch", CategoryId = catPaint, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "PA012", Name = "Paint Brush 3 inch", CategoryId = catPaint, PurchasePrice = 350, SellingPrice = 520, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "PA013", Name = "Roller Set 9 inch", CategoryId = catPaint, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 180, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "PA014", Name = "Paint Thinner 1L", CategoryId = catPaint, PurchasePrice = 350, SellingPrice = 520, CurrentStock = 120, ReorderLevel = 40, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Safety
            products.AddRange(new[] {
                new Product { Code = "SF001", Name = "Safety Helmet White", CategoryId = catSafety, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 150, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "SF002", Name = "Safety Helmet Yellow", CategoryId = catSafety, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 120, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "SF003", Name = "Safety Helmet Red", CategoryId = catSafety, PurchasePrice = 550, SellingPrice = 800, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "SF004", Name = "Safety Goggles Clear", CategoryId = catSafety, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 300, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "SF005", Name = "Safety Gloves Leather", CategoryId = catSafety, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "SF006", Name = "Safety GlovesCotton", CategoryId = catSafety, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 400, ReorderLevel = 120, SupplierId = 1 },
                new Product { Code = "SF007", Name = "Ear Muffs", CategoryId = catSafety, PurchasePrice = 350, SellingPrice = 520, CurrentStock = 100, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "SF008", Name = "Dust Mask 3Ply Box", CategoryId = catSafety, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 500, ReorderLevel = 150, SupplierId = 1 },
                new Product { Code = "SF009", Name = "Safety Vest Orange", CategoryId = catSafety, PurchasePrice = 650, SellingPrice = 950, CurrentStock = 120, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "SF010", Name = "Safety Vest Yellow", CategoryId = catSafety, PurchasePrice = 650, SellingPrice = 950, CurrentStock = 100, ReorderLevel = 35, SupplierId = 1 },
                new Product { Code = "SF011", Name = "Steel Toe Boots Size 8", CategoryId = catSafety, PurchasePrice = 2500, SellingPrice = 3800, CurrentStock = 40, ReorderLevel = 15, SupplierId = 1 },
                new Product { Code = "SF012", Name = "Steel Toe Boots Size 9", CategoryId = catSafety, PurchasePrice = 2500, SellingPrice = 3800, CurrentStock = 35, ReorderLevel = 12, SupplierId = 1 },
                new Product { Code = "SF013", Name = "Steel Toe Boots Size 10", CategoryId = catSafety, PurchasePrice = 2500, SellingPrice = 3800, CurrentStock = 30, ReorderLevel = 10, SupplierId = 1 },
                new Product { Code = "SF014", Name = "First Aid Kit", CategoryId = catSafety, PurchasePrice = 1200, SellingPrice = 1750, CurrentStock = 50, ReorderLevel = 15, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
            products.Clear();
            
            // Hardware (last batch)
            products.AddRange(new[] {
                new Product { Code = "HW001", Name = "Door Hinge Brass 3 inch", CategoryId = catHardware, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 400, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "HW002", Name = "Door Handle Set", CategoryId = catHardware, PurchasePrice = 1200, SellingPrice = 1750, CurrentStock = 80, ReorderLevel = 25, SupplierId = 1 },
                new Product { Code = "HW003", Name = "Mortise Lock", CategoryId = catHardware, PurchasePrice = 850, SellingPrice = 1250, CurrentStock = 60, ReorderLevel = 20, SupplierId = 1 },
                new Product { Code = "HW004", Name = "Cabinet Knob 25mm", CategoryId = catHardware, PurchasePrice = 120, SellingPrice = 180, CurrentStock = 350, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "HW005", Name = "Drawer Pull 96mm", CategoryId = catHardware, PurchasePrice = 150, SellingPrice = 220, CurrentStock = 280, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "HW006", Name = "Tower Bolt 6 inch", CategoryId = catHardware, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 250, ReorderLevel = 70, SupplierId = 1 },
                new Product { Code = "HW007", Name = "Gate Hook 4 inch", CategoryId = catHardware, PurchasePrice = 150, SellingPrice = 220, CurrentStock = 180, ReorderLevel = 50, SupplierId = 1 },
                new Product { Code = "HW008", Name = "Eye Bolt 100mm", CategoryId = catHardware, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "HW009", Name = "Padlock 40mm", CategoryId = catHardware, PurchasePrice = 350, SellingPrice = 520, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "HW010", Name = "Padlock 50mm", CategoryId = catHardware, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 150, ReorderLevel = 45, SupplierId = 1 },
                new Product { Code = "HW011", Name = "Cable Ties 100pc", CategoryId = catHardware, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 400, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "HW012", Name = "Cable Clips 100pc", CategoryId = catHardware, PurchasePrice = 120, SellingPrice = 180, CurrentStock = 350, ReorderLevel = 100, SupplierId = 1 },
                new Product { Code = "HW013", Name = "Steel Wool 400g", CategoryId = catHardware, PurchasePrice = 250, SellingPrice = 380, CurrentStock = 200, ReorderLevel = 60, SupplierId = 1 },
                new Product { Code = "HW014", Name = "Sandpaper 120 grit 10pc", CategoryId = catHardware, PurchasePrice = 280, SellingPrice = 420, CurrentStock = 300, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "HW015", Name = "Sandpaper 220 grit 10pc", CategoryId = catHardware, PurchasePrice = 280, SellingPrice = 420, CurrentStock = 280, ReorderLevel = 80, SupplierId = 1 },
                new Product { Code = "HW016", Name = "Hack Blade 24T", CategoryId = catHardware, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "HW017", Name = "Hack Blade 32T", CategoryId = catHardware, PurchasePrice = 450, SellingPrice = 650, CurrentStock = 120, ReorderLevel = 35, SupplierId = 1 },
                new Product { Code = "HW018", Name = "Measuring Cup 500ml", CategoryId = catHardware, PurchasePrice = 280, SellingPrice = 420, CurrentStock = 100, ReorderLevel = 30, SupplierId = 1 },
                new Product { Code = "HW019", Name = "Paint Tray", CategoryId = catHardware, PurchasePrice = 180, SellingPrice = 270, CurrentStock = 150, ReorderLevel = 40, SupplierId = 1 },
                new Product { Code = "HW020", Name = "Extension Cord 15m", CategoryId = catHardware, PurchasePrice = 1500, SellingPrice = 2200, CurrentStock = 60, ReorderLevel = 20, SupplierId = 1 },
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        var adminEmail = "admin@hsms.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                FullName = "System Administrator",
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        var staffEmail = "staff@hsms.com";
        if (await userManager.FindByEmailAsync(staffEmail) == null)
        {
            var staff = new ApplicationUser
            {
                FullName = "Shop Staff",
                Email = staffEmail,
                UserName = staffEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            var result = await userManager.CreateAsync(staff, "Staff123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(staff, "Staff");
            }
        }
    }
}