using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMOffersClients
{
    public class Client
    {
        /// <summary>
        /// The Id of the client
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the client
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The discount for that client
        /// </summary>
        public double Discount { get; set; }

        /// <summary>
        /// The city where the client is from
        /// </summary>
        [Required]
        [StringLength(50)]
        public string City { get; set; }

        /// <summary>
        /// The other end of the <see cref="Offers.ClientId"/> foreign key
        /// </summary>
        [InverseProperty("Client")]
        public virtual ICollection<Offers> Offers { get; set; }
    }
}