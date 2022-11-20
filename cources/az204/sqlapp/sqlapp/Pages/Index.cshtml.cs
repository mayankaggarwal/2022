using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sqlapp.Models;
using sqlapp.Services;

namespace sqlapp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProductService _productService;

        public IndexModel(IProductService productService, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _productService= productService;
            Products= new List<Product>();
        }

        public List<Product> Products;
        public void OnGet()
        {
            _logger.LogInformation("Index Model On Get");
            Products = _productService.GetProducts();
        }
    }
}