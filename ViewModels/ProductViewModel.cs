using System.ComponentModel.DataAnnotations;

namespace TMOffersClients
{
    /// <summary>
    /// The view model for a <see cref="Product"/> object
    /// </summary>
    public class ProductViewModel
    {

        /// <summary>
        /// The Id of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The order number
        /// </summary>
        [Required]
        public string OrderNumber { get; set; }

        /// <summary>
        /// The price unit for this product
        /// </summary>
        public string PriceUnit { get; set; }

        /// <summary>
        /// The price of this product
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// The category of the product
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }
    }
}
