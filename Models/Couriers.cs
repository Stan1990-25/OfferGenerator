using System.ComponentModel.DataAnnotations;

namespace TMOffersClients
{
    public class Couriers
    {
        /// <summary>
        /// The Id of the <see cref="Client"/>
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The name of the Courier
        /// </summary>
        [Key]
        [StringLength(50)]
        public string CourierName { get; set; }
    }
}