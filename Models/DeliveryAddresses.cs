using System.ComponentModel.DataAnnotations;

namespace TMOffersClients
{
    public class DeliveryAddresses
    {
        /// <summary>
        /// The Id of the <see cref="Client"/>
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The delivery address for that client
        /// </summary>
        [Key]
        [StringLength(100)]
        public string DelAddress { get; set; }
    }
}