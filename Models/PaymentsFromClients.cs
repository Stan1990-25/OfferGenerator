using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMOffersClients
{
    /// <summary>
    /// Table that will calculate the payments from the clients
    /// </summary>
    public class PaymentsFromClients
    {
        /// <summary>
        /// Id of the payment
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The offer number
        /// </summary>
        [StringLength(30)]
        public string OfferNumber { get; set; }

        /// <summary>
        /// The name of the client
        /// </summary>
        [StringLength(50)]
        public string ClientName { get; set; }

        /// <summary>
        /// The total price of the offer
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal OfferTotal { get; set; }

        /// <summary>
        /// The value of the payment done
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentValue { get; set; }

        /// <summary>
        /// The remaining value of the payments
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal RemainingValue { get; set; }
    }
}
