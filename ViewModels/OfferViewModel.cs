using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TMOffersClients.ViewModels
{
    public class OfferViewModel
    {
        [Required]
        public string Number { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string ClientCity { get; set; }
        [Required]
        public string ClientPhoneNumber { get; set; }
        public int ClientId { get; set; }
        [Required]
        public double Discount { get; set; }
        [Required]
        public string ContactName { get; set; }
        public List<OrderProduct> SelectedProducts { get; set; } = new List<OrderProduct>();
        [Required]
        public int ExpirationDays { get; set; }
        [Required]
        public string DeliveryTerms { get; set; }
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public string DeliveryDeadline { get; set; }
        [Required]
        public string DeliveryType { get; set; }
        public decimal Total { get; set; }
        [Required] 
        public string Author { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public bool IsApproved { get; set; }
        public List<Product> AllProducts { get; set; } = new List<Product>();
        public ListClientsViewModel AllClients { get; set; } = new ListClientsViewModel();
    }

    public class ListOffersViewModel
    {
        public List<OfferViewModel> Offers { get; set; } = new List<OfferViewModel>();
        public ListClientsViewModel AllClients { get; set; } = new ListClientsViewModel();
    }

    public class OrderProduct
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Quantity { get; set; }
        public double Meters { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Category == "Профили" ? UnitPrice * (decimal)Quantity * (decimal)Meters : UnitPrice * (decimal)Quantity;
    }
}
