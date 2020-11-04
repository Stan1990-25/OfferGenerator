using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TMOffersClients.HelperClasses;
using TMOffersClients.Models;
using TMOffersClients.ViewModels;

namespace TMOffersClients.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        #region Private members

        /// <summary>
        /// Injecting the <see cref="ProductRepository"/> class on every request
        /// </summary>
        private readonly IProductRepository mProductRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// Injecting the <see cref="ProductRepository"/> class on every request
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductsController(IProductRepository productRepository)
        {
            mProductRepository = productRepository;
        }

        #endregion

        #region Action Methods

        /// <summary>
        /// Return view with all the products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ListProducts()
        {
            List<Product> products = mProductRepository.GetAllProducts().ToList();
            return View(products);
        } 

        /// <summary>
        /// Return view for adding product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View(new ProductViewModel());
        }

        /// <summary>
        /// Adding product to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddProduct([FromBody]ProductViewModel model)
        {
            Product product = new Product
            {
                OrderNumber = model.OrderNumber,
                PriceUnit = model.PriceUnit,
                Price = model.Price,
                Category = model.Category,
                Description = model.Description
            };

            mProductRepository.AddProduct(product);
            return Json(new { newUrl = Url.Action("AddProduct", "Products") });
        }

        /// <summary>
        /// Return view with specified products of certain category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetProductsByCategory(string category)
        {
            List<Product> products = mProductRepository.GetProductsByCategory(category).ToList();
            return View("ListProducts", products);
        }

        /// <summary>
        /// Return Update view for a certain product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            Product product = mProductRepository.GetProductById(id);

            ProductViewModel model = new ProductViewModel
            {
                Id = product.Id,
                OrderNumber = product.OrderNumber,
                PriceUnit = product.PriceUnit,
                Price = product.Price,
                Category = product.Category,
                Description = product.Description
            };

            return View(model);
        }

        /// <summary>
        /// Adding product to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateProduct(ProductViewModel model)
        {
            Product product = new Product
            {
                Id = model.Id,
                OrderNumber = model.OrderNumber,
                PriceUnit = model.PriceUnit,
                Price = model.Price,
                Category = model.Category,
                Description = model.Description
            };

            mProductRepository.UpdateProduct(product);
            return Json(new { newUrl = Url.Action("ListProducts", "Products") });
        }

        /// <summary>
        /// Delete product from database
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        public IActionResult DeleteProduct(int id)
        {
            mProductRepository.DeleteProduct(id);

            return RedirectToAction(nameof(ListProducts));
        }

        public IActionResult ExportPDF()
        {
            var offer = new OfferViewModel
            {
                Number = "ВН20-0029-01/20.07.20",
                ClientName = "Валентин Желев",
                ClientCity = "Шумен",
                ClientPhoneNumber = "0886 524 381",
                ContactName = "Валентин Желев",
                SelectedProducts = new List<OrderProduct>
                {
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0850", Description = "Профил 40x40", UnitPrice = 21.60M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0600", Description = "Профил 40x40", UnitPrice = 15.25M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "PIL4040SNN0520", Description = "Профил 40x40", UnitPrice = 13.21M, Quantity = 6 },
                    new OrderProduct { ProductNumber = "FAS4041", Description = "Сглобка 40x40, комплект", UnitPrice = 10.23M, Quantity = 16 },
                    new OrderProduct { ProductNumber = "DOR4501", Description = "Ръкохватка, комплект", UnitPrice = 13.02M, Quantity = 2 },
                    new OrderProduct { ProductNumber = "CAS3080", Description = "Направляваща ролка с отвор от заднат", UnitPrice = 14.20M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "CAP4041", Description = "Капачка 40х40, комплект", UnitPrice = 2.72M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "IN4506", Description = "Позиционираща се гайка M6", UnitPrice = 2.39M, Quantity = 4 },
                    new OrderProduct { ProductNumber = "TIN4504", Description = "Позиционираща се гайка M4", UnitPrice = 2.39M, Quantity = 24 },
                }
                
            };

            // OfferGeneratorPDF.GeneratePDF(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), offer, Startup.Environment);

            GeneratePDFOffer.CreatePDF(offer, Startup.Environment);

            return RedirectToAction("ListProducts");
        }

        #endregion
    }
}