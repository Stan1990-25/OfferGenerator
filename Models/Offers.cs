using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMOffersClients
{
    public class Offers
    {
        /// <summary>
        /// The offer number
        /// </summary>
        [Key]
        [StringLength(30)]
        public string OfferNumber { get; set; }

        /// <summary>
        /// The Id of the <see cref="Client"/>
        /// </summary>
        [Required]
        public int ClientId { get; set; }

        /// <summary>
        /// The name of the contact person
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ContactName { get; set; }

        /// <summary>
        /// Dsicount for the current offer
        /// </summary>
        [Required]
        public double Discount { get; set; }

        /// <summary>
        /// Validity of the offer
        /// </summary>
        [Required]
        public int ExpirationDays { get; set; }

        /// <summary>
        /// Delivery terms
        /// </summary>
        [Required]
        [StringLength(200)]
        public string DeliveryTerms { get; set; }

        /// <summary>
        /// Payment type
        /// </summary>
        [Required]
        [StringLength(200)]
        public string PaymentType { get; set; }

        /// <summary>
        /// Delivery deadline
        /// </summary>
        [Required]
        [StringLength(200)]
        public string DeliveryDeadline { get; set; }

        /// <summary>
        /// Delivery type
        /// </summary>
        [Required]
        [StringLength(200)]
        public string DeliveryType { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Author { get; set; }

        /// <summary>
        /// Date Modified
        /// </summary>
        [Required]
        public DateTimeOffset DateModified { get; set; }

        /// <summary>
        /// Total price of the offer
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Indicates whether this offer/revision is approved by the client
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Foreign key to the client table
        /// </summary>
        public Client Client { get; set; }
    }
}