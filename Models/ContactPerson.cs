using System.ComponentModel.DataAnnotations;

namespace TMOffersClients
{
    public class ContactPerson
    {
        /// <summary>
        /// The Id of the <see cref="Client"/>
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the <see cref="ContactPerson"/>
        /// </summary>
        [Key]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The phone number of the <see cref="ContactPerson"/>
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// The Email address of the <see cref="ContactPerson"/>
        /// </summary>
        public string EmailAddress { get; set; }
    }
}