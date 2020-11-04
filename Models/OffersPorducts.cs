namespace TMOffersClients
{
    public class OffersPorducts
    {
        /// <summary>
        /// The Id of the offer
        /// </summary>
        public string OfferID { get; set; }

        /// <summary>
        /// The Id of the product
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Quantity of the product
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Meters for the profiles
        /// </summary>
        public double Meters { get; set; }
    }
}
