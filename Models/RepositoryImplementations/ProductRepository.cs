using System.Collections.Generic;
using System.Linq;

namespace TMOffersClients.Models
{
    /// <summary>
    /// The class that will be serviced with the Dependency Injection for 
    /// operations with the <see cref="Product"/> class
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        #region Private Members

        /// <summary>
        /// The database instance
        /// </summary>
        private readonly AppDbContext mDBContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// Injecting the database provider.
        /// </summary>
        /// <param name="dbContext"></param>
        public ProductRepository(AppDbContext dbContext)
        {
            mDBContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adding a product in the database
        /// </summary>
        /// <param name="product">The <see cref="Product"/> object</param>
        /// <returns></returns>
        public Product AddProduct(Product product)
        {
            mDBContext.Products.Add(product);
            mDBContext.SaveChanges();
            return product;
        }

        /// <summary>
        /// Deleting a product from the database
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        public Product DeleteProduct(int id)
        {
            var productForDeletion = mDBContext.Products.Find(id);

            if (productForDeletion == null)
                return null;

            mDBContext.Products.Remove(productForDeletion);

            mDBContext.SaveChanges();

            return productForDeletion;
        }

        /// <summary>
        /// Get all the products from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return mDBContext.Products.OrderBy(p => p.Category).ToList();
        }

        /// <summary>
        /// Get the products from certain category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return mDBContext.Products.Where(p => p.Category == category).ToList();
        }
        
        /// <summary>
        /// Get single product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetProductById(int id)
        {
            return mDBContext.Products.Find(id);
        }

        /// <summary>
        /// Get single product by order number
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public Product GetProductByOrderNumber(string orderNumber)
        {
            var product = mDBContext.Products.Where(p => p.OrderNumber == orderNumber).ToList();

            if (product.Count > 0)
                return product[0];

            return null;
        }

        /// <summary>
        /// Update product in the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product UpdateProduct(Product product)
        {
            var currProduct = mDBContext.Products.Find(product.Id);

            currProduct.OrderNumber = product.OrderNumber;
            currProduct.PriceUnit = product.PriceUnit;
            currProduct.Price = product.Price;
            currProduct.Category = product.Category;
            currProduct.Description = product.Description;

            mDBContext.Products.Update(currProduct);

            mDBContext.SaveChanges();

            return currProduct;
        } 
        
        #endregion
    }
}
