using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TMOffersClients.Models
{
    /// <summary>
    /// The class for the DI representing the <see cref="Client"/> class
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        #region Private members

        /// <summary>
        /// The database object
        /// </summary>
        private readonly AppDbContext mDBContext;

        /// <summary>
        /// Dependency Injection implementation class for <see cref="ContactPerson"/>
        /// </summary>
        private readonly IContactPersonRepository mContactPersons;

        private readonly ICouriersRepository mCouriers;
        private readonly IDeliveryRepository mDeliveryAddresses;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public ClientRepository(AppDbContext dbContext, IContactPersonRepository contacts, 
                                ICouriersRepository couriers, IDeliveryRepository deliveryAddresses)
        {
            mDBContext = dbContext;
            mContactPersons = contacts;
            mCouriers = couriers;
            mDeliveryAddresses = deliveryAddresses;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adding a new client to the database
        /// </summary>
        /// <param name="client"></param>
        /// <param name="contacts"></param>
        /// <param name="deliveryAddresses"></param>
        /// <param name="couriers"></param>
        /// <returns></returns>
        public bool AddClient(Client client, List<ContactPerson> contacts, List<DeliveryAddresses> deliveryAddresses, List<Couriers> couriers)
        {
            // Result from the adding
            bool clientAddedSucc = false;

            mDBContext.Clients.Add(client);
            mDBContext.SaveChanges();

            Client writtenClient = mDBContext.Clients.Where(cl => cl.Name == client.Name).First();

            if (contacts.Count > 0)
            {
                for (int i = 0; i < contacts.Count; i++)
                {
                    contacts[i].Id = writtenClient.Id;
                }
                mDBContext.ContactPersons.AddRange(contacts);
            }
            if (deliveryAddresses.Count > 0)
            {
                for (int i = 0; i < deliveryAddresses.Count; i++)
                {
                    deliveryAddresses[i].Id = writtenClient.Id;
                }
                mDBContext.DeliveryAddresses.AddRange(deliveryAddresses);
            }

            if (couriers.Count > 0)
            {
                for (int i = 0; i < couriers.Count; i++)
                {
                    couriers[i].Id = writtenClient.Id;
                }
                mDBContext.Couriers.AddRange(couriers);
            }

            try
            {
                mDBContext.SaveChanges();
            }
            catch (DbUpdateException x)
            {
                throw new DbUpdateException("Adding a client to the database failed. " + x.Message);
            }
            catch (DBConcurrencyException x)
            {
                throw new DbUpdateException("Adding a client to the database failed. " + x.Message);
            }

            clientAddedSucc = true;
            return clientAddedSucc;
        }

        /// <summary>
        /// Delete client from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteClient(int id)
        {
            // Remove all
            // Contacts
            var currContacts = mDBContext.ContactPersons.Where(cp => cp.Id == id).ToList();
            mDBContext.ContactPersons.RemoveRange(currContacts);

            // Addresses
            var currAddresses = mDBContext.DeliveryAddresses.Where(cp => cp.Id == id).ToList();
            mDBContext.DeliveryAddresses.RemoveRange(currAddresses);

            // Couriers
            var currCouriers = mDBContext.Couriers.Where(cp => cp.Id == id).ToList();
            mDBContext.Couriers.RemoveRange(currCouriers);

            var client = mDBContext.Clients.Find(id);
            mDBContext.Clients.Remove(client);

            try
            {
                mDBContext.SaveChanges();
            }
            catch (DbUpdateException x)
            {
                throw new DbUpdateException("Adding a client to the database failed. " + x.Message);
            }
            catch (DBConcurrencyException x)
            {
                throw new DbUpdateException("Adding a client to the database failed. " + x.Message);
            }

            return true;
        }

        /// <summary>
        /// Get client by Id
        /// </summary>
        /// <param name="id">The id for the client</param>
        /// <returns></returns>
        public Client GetClient(int id)
        {
            return mDBContext.Clients.Find(id);
        }

        /// <summary>
        /// Get client by name
        /// </summary>
        /// <param name="name">The name of the client to be searched</param>
        /// <returns></returns>
        public Client GetClient(string name)
        {
            List<Client> clients = new List<Client>();
            clients = mDBContext.Clients.Where(c => c.Name == name).Select(c => c).ToList();
            if (clients.Count == 0)
                return null;
            else
                return clients[0];
        }

        /// <summary>
        /// Get all clients from the database
        /// </summary>
        /// <returns></returns>
        public List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            clients = mDBContext.Clients.ToList();

            return clients;
        }

        /// <summary>
        /// Updating the information about the client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool UpdateClient(Client client, List<ContactPerson> contacts, List<DeliveryAddresses> deliveryAddresses, List<Couriers> couriers)
        {
            Client updatedClient = GetClient(client.Id);

            updatedClient.Name = client.Name;
            updatedClient.Discount = client.Discount;
            updatedClient.City = client.City;

            mDBContext.Clients.Update(updatedClient);

            // First remove all then add the new
            // Contacts
            var currContacts = mDBContext.ContactPersons.Where(cp => cp.Id == client.Id).ToList();
            mDBContext.ContactPersons.RemoveRange(currContacts);
            if (contacts.Count > 0)
                mDBContext.ContactPersons.AddRange(contacts);

            // Addresses
            var currAddresses = mDBContext.DeliveryAddresses.Where(cp => cp.Id == client.Id).ToList();
            mDBContext.DeliveryAddresses.RemoveRange(currAddresses);
            if (deliveryAddresses.Count > 0)
                mDBContext.DeliveryAddresses.AddRange(deliveryAddresses);

            // Couriers
            var currCouriers = mDBContext.Couriers.Where(cp => cp.Id == client.Id).ToList();
            mDBContext.Couriers.RemoveRange(currCouriers);
            if (couriers.Count > 0)
                mDBContext.Couriers.AddRange(couriers);

            try
            {
                mDBContext.SaveChanges();
            }
            catch (DbUpdateException x)
            {
                throw new DbUpdateException("Adding a client to the database failed. " + x.Message);
            }
            catch (DBConcurrencyException x)
            {
                throw new DbUpdateException("Adding a client to the database failed. " + x.Message);
            }

            return true;
        }

        /// <summary>
        /// Search client in the database
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Client> SearchClient(string search)
        {
            var clients = mDBContext.Clients.ToList();
            return clients.Where(cl => StringHelpers.Contains(cl.Name, search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        #endregion



        #region Helper Methods



        #endregion
    }
}
