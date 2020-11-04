using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMOffersClients.ViewModels;

namespace TMOffersClients.Models
{
    #region Client related repositories

    /// <summary>
    /// The interface for the Dependency Injection that the <see cref="Client"/> class will implement
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Get a client by Id
        /// </summary>
        /// <param name="id">The Id to be searched for</param>
        /// <returns></returns>
        Client GetClient(int id);

        /// <summary>
        /// Get a client by name
        /// </summary>
        /// <param name="name">The name to be searched for</param>
        /// <returns></returns>
        Client GetClient(string name);
        
        /// <summary>
        /// Get all clients from the database
        /// </summary>
        /// <returns></returns>
        List<Client> GetAllClients();

        /// <summary>
        /// Add a client to the database
        /// </summary>
        /// <param name="client">The <see cref="Client"/> object to add</param>
        /// <returns></returns>
        bool AddClient(Client client, List<ContactPerson> contacts, List<DeliveryAddresses> deliveryAddresses, List<Couriers> couriers);

        /// <summary>
        /// Delete client from the database
        /// </summary>
        /// <param name="id">The id of the <see cref="Client"/> object to delete</param>
        /// <returns></returns>
        bool DeleteClient(int id);

        /// <summary>
        /// Changing the data of the client
        /// </summary>
        /// <param name="client">The <see cref="Client"/> object to update</param>
        /// <returns></returns>
        bool UpdateClient(Client client, List<ContactPerson> contacts, List<DeliveryAddresses> deliveryAddresses, List<Couriers> couriers);

        /// <summary>
        /// Search client by name
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Client> SearchClient(string search);
    }

    /// <summary>
    /// The interface for the Dependency Injection that the <see cref="Client"/> class will implement
    /// </summary>
    public interface IContactPersonRepository
    {
        /// <summary>
        /// Get the contact persons for the specific client
        /// </summary>
        /// <param name="id">The Id to be searched for</param>
        /// <returns></returns>
        Task<IEnumerable<ContactPerson>> GetContacts(int id);

        /// <summary>
        /// Get single contact via name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ContactPerson GetContact(string name);

        /// <summary>
        /// Add a contact person to the database
        /// </summary>
        /// <param name="contact">The <see cref="ContactPerson"/> object to add</param>
        /// <returns></returns>
        bool AddContact(ContactPerson contact);

        /// <summary>
        /// Delete contact person from the database
        /// </summary>
        /// <param name="id">The id of the <see cref="ContactPerson"/> object to delete</param>
        /// <param name="name">The name of the <see cref="ContactPerson"/> to delete</param>
        /// <returns></returns>
        bool DeleteContact(int id, string name);

        /// <summary>
        /// Changing the data of the contact person
        /// </summary>
        /// <param name="contacts">The list of <see cref="ContactPerson"/> to update</param>
        /// <returns></returns>
        List<ContactPerson> UpdateContacts(List<ContactPerson> contacts);
    }

    /// <summary>
    /// The interface for the Dependency Injection that the <see cref="Client"/> class will implement
    /// </summary>
    public interface ICouriersRepository
    {
        /// <summary>
        /// Get the couriers for the specific client
        /// </summary>
        /// <param name="id">The Id of the client</param>
        /// <returns></returns>
        Task<IEnumerable<Couriers>> GetCouriers(int id);

        /// <summary>
        /// Add a courier to the database
        /// </summary>
        /// <param name="contact">The <see cref="Couriers"/> object to add</param>
        /// <returns></returns>
        bool AddCourier(Couriers courier);

        /// <summary>
        /// Delete courier from the database
        /// </summary>
        /// <param name="id">The id of the <see cref="Couriers"/> object to delete</param>
        /// <param name="courierName">The name of the <see cref="Couriers"/> to delete</param>
        /// <returns></returns>
        bool DeleteCourier(int id, string courierName);

        /// <summary>
        /// Updating the list of couriers
        /// </summary>
        /// <param name="couriers">The list of <see cref="Couriers"/> to update</param>
        /// <returns></returns>
        List<Couriers> UpdateCouriers(List<Couriers> couriers);
    }

    /// <summary>
    /// The interface for the Dependency Injection that the <see cref="Client"/> class will implement
    /// </summary>
    public interface IDeliveryRepository
    {
        /// <summary>
        /// Get the delivery addresses for the specific client
        /// </summary>
        /// <param name="id">The Id of the client</param>
        /// <returns></returns>
        Task<IEnumerable<DeliveryAddresses>> GetAddresses(int id);

        /// <summary>
        /// Add a delivery address to the database for the specific client
        /// </summary>
        /// <param name="deliveryAddress">The <see cref="DeliveryAddresses"/> object to add</param>
        /// <returns></returns>
        bool AddAddress(DeliveryAddresses deliveryAddress);

        /// <summary>
        /// Delete delivery address from the database for a specific client
        /// </summary>
        /// <param name="id">The id of the <see cref="DeliveryAddresses"/> object to delete</param>
        /// <param name="address">The address of the <see cref="DeliveryAddresses"/> to delete</param>
        /// <returns></returns>
        bool DeleteAddress(int id, string address);

        /// <summary>
        /// Changing the data of the delivery address for a specific client
        /// </summary>
        ///<param name="deliveryAddresses">The list of the <see cref="DeliveryAddresses"/> to update</param>
        /// <returns></returns>
        List<DeliveryAddresses> UpdateAddresses(List<DeliveryAddresses> deliveryAddresses);
    }

    #endregion

    #region Product related repositories

    /// <summary>
    /// The interface for the Dependency Injection that the <see cref="Product"/> class will use
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Return all products
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetAllProducts();

        /// <summary>
        /// Return product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProductById(int id);

        /// <summary>
        /// Return product by order number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProductByOrderNumber(string orderNumber);

        /// <summary>
        /// Return products by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<Product> GetProductsByCategory(string category);

        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Product AddProduct(Product product);

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product DeleteProduct(int id);

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product UpdateProduct(Product product);
    }

    #endregion

    #region Offer Related Repositories

    /// <summary>
    /// The interface for the Dependency Injection that the <see cref="Offers"/> class will implement
    /// </summary>
    public interface IOffersRepository
    {
        /// <summary>
        /// Get a list of all recorded offers
        /// </summary>
        /// <returns></returns>
        List<Offers> GetAllOffers();

        /// <summary>
        /// Get all the revisions for the offer
        /// </summary>
        /// <returns></returns>
        List<Offers> GettAllOfferRevisions(string id);

        /// <summary>
        /// Get all the products that an offer has
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        List<OffersPorducts> GetAllProductsForOffer(string offerId);

        /// <summary>
        /// Get single offer
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        Offers GetOffer(string offerId);

        /// <summary>
        /// Add offer to the database
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        Offers AddOffer(Offers offer, List<OrderProduct> products);

        /// <summary>
        /// Change offer in the database
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        Offers ChangeOffer(Offers offer, List<OrderProduct> products);

        /// <summary>
        /// Delete offer from the database
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        Offers DeleteOffer(string offerId);

        /// <summary>
        /// Get the last offer from the database
        /// </summary>
        /// <returns></returns>
        string GetLastOffer();

        /// <summary>
        /// Approve offer
        /// </summary>
        /// <param name="id"></param>
        Task ApproveOffer(string id);

        /// <summary>
        /// Approve offer
        /// </summary>
        /// <param name="id"></param>
        Task DisApproveOffer(string id);
    }
    
    #endregion
}
