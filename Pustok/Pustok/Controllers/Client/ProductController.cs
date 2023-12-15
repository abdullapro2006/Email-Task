using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Extensions;
using Pustok.Services.Abstract;
using Pustok.ViewModels.Product;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers.Client;

public class ProductController : Controller
{
    private readonly PustokDbContext _pustokdbContext;
    private readonly IFileService _fileService;

    public ProductController(PustokDbContext pustokdbContext, 
        IFileService fileService)
    {
        _pustokdbContext = pustokdbContext;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var productsPageViewModel = new ProductsPageViewModel {

            Products = await _pustokdbContext.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Rating = p.Rating,
                ImageUrl = UploadDirectory.Products.GetUrl(p.ImageNameInFileSystem)
            })
            .ToListAsync(),

            Categories = await _pustokdbContext.Categories
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductsCount = c.Products.Count,

            } )
            .ToListAsync(),

            Colors = await _pustokdbContext.Colors
            .Select(c => new ColorViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ProductsCount = c.ProductColors.Count,

            })
            .ToListAsync(),

            PriceMinRange =  _pustokdbContext.Products.OrderBy(p => p.Price).FirstOrDefault()?.Price,
            PriceMaxRange = _pustokdbContext.Products.OrderByDescending(p => p.Price).FirstOrDefault()?.Price,
        
        };

        return View(productsPageViewModel);
    }

    public IActionResult SingleProduct(int id, [FromServices] PustokDbContext pustokDbContext)
    {
        var product = pustokDbContext.Products
            .Include(p => p.Category)
            .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public IActionResult ProductDetails(int id, [FromServices] PustokDbContext pustokDbContext)
    {
        var product = pustokDbContext.Products
            .Include(p => p.Category)
            .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var productViewModel = new ProductDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Rating = product.Rating,
            CategoryName = product.Category.Name,
            Colors = product.ProductColors
                .Select(pc =>
                    new ProductDetailsViewModel.ColorDetailsViewModel
                    {
                        Id = pc.ColorId,
                        Name = pc.Color.Name
                    })
                .ToList(),
            Sizes = product.ProductSizes
                .Select(pc =>
                    new ProductDetailsViewModel.SizeDetailsViewModel
                    {
                        Id = pc.SizeId,
                        Name = pc.Size.Name
                    })
                .ToList()
        };

        return Json(productViewModel);
    }

    public IActionResult ProductDetailsModalView(int id, [FromServices] PustokDbContext pustokDbContext)
    {
        var product = pustokDbContext.Products
            .Include(p => p.Category)
            .Include(p => p.ProductSizes)
                .ThenInclude(ps => ps.Size)
            .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var productViewModel = new ProductDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Rating = product.Rating,
            CategoryName = product.Category.Name,
            ImageNameInFileSystem = product.ImageNameInFileSystem,
            Colors = product.ProductColors
                .Select(pc =>
                    new ProductDetailsViewModel.ColorDetailsViewModel
                    {
                        Id = pc.ColorId,
                        Name = pc.Color.Name
                    })
                .ToList(),
            Sizes = product.ProductSizes
                .Select(pc =>
                    new ProductDetailsViewModel.SizeDetailsViewModel
                    {
                        Id = pc.SizeId,
                        Name = pc.Size.Name
                    })
                .ToList()
        };

        return PartialView("Partials/Client/_ProductDetailsModalBodyPartialView", productViewModel);
    }
}
