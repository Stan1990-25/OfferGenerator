using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace TMOffersClients
{
    public class Product
    {
        /// <summary>
        /// The Id of the product
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Order number of the product
        /// </summary>
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        /// <summary>
        /// The measuring unit to calculate the price
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PriceUnit { get; set; }

        /// <summary>
        /// The price of the product
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// The category of the product
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        /// <summary>
        /// Description for the product
        /// </summary>
        [StringLength(512)]
        public string Description { get; set; }

    }
}