using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeduCoreApp.Models.ProductViewModels;
using TeduCoreApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TeduCoreApp.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;
        IConfiguration _configuration;
        public ProductController(IProductService productService,IConfiguration configuration,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
        }
        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(id, string.Empty, page, pageSize.Value);
            catalog.Category = _productCategoryService.GetById(id);

            return View(catalog);
        }

        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            return View();
        }

    }
}