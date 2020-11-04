using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMOffersClients.Models
{
    public class DeliveryAddressesRepository : IDeliveryRepository
    {
        #region Private members

        /// <summary>
        /// The database object
        /// </summary>
        private readonly AppDbContext mDBContext;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext">The database context</param>
        public DeliveryAddressesRepository(AppDbContext dbContext)
        {
            mDBContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add new delivery address to the database
        /// </summary>
        /// <param name="deliveryAddress">The <see cref="DeliveryAddresses"/> object to add</param>
        /// <returns></returns>
        public bool AddAddress(DeliveryAddresses deliveryAddress)
        {
            mDBContext.DeliveryAddresses.Add(deliveryAddress);
            return true;
        }

        /// <summary>
        /// Delete delivery address from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool DeleteAddress(int id, string address)
        {
            // Output whether the queries should be sent to the database
            bool saveRequest = false;

            // Find the contact in the database
            var deliveryAddress = mDBContext.DeliveryAddresses.Find(id, address);

            if (deliveryAddress == null)
                return saveRequest;

            mDBContext.DeliveryAddresses.Remove(deliveryAddress);

            saveRequest = true;

            return saveRequest;
        }

        /// <summary>
        /// Get all delivery addresses form the database for a specific client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DeliveryAddresses>> GetAddresses(int id)
        {
            List<DeliveryAddresses> addresses = new List<DeliveryAddresses>();
            // Get all couriers with this Id
            await Task.Run(() =>
            {
                addresses = mDBContext.DeliveryAddresses.Where((da) => da.Id == id).ToList();
            });

            return addresses;
        }

        /// <summary>
        /// Updating delivery address in the database
        /// </summary>
        /// <param name="deliveryAddress"></param>
        /// <returns></returns>
        public List<DeliveryAddresses> UpdateAddresses(List<DeliveryAddresses> deliveryAddresses)
        {
            // Find the contact in the database
            var addressesForUpdate = mDBContext.DeliveryAddresses.Where(da => da.Id == deliveryAddresses[0].Id).ToList();

            if (addressesForUpdate == null || addressesForUpdate.Count == 0)
                return null;

            addressesForUpdate = deliveryAddresses;

            return addressesForUpdate;
        }

        #endregion
    }
}
