using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMOffersClients.Models
{
    /// <summary>
    /// The class for the DI representing the <see cref="Couriers"/> class
    /// </summary>
    public class CouriersRepository : ICouriersRepository
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
        public CouriersRepository(AppDbContext dbContext)
        {
            mDBContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a courier to the database
        /// </summary>
        /// <param name="courier">The <see cref="Couriers"/> object to be added</param>
        /// <returns></returns>
        public bool AddCourier(Couriers courier)
        {
            mDBContext.Couriers.Add(courier);
            return true;
        }

        /// <summary>
        /// Delete a courier from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courierName"></param>
        /// <returns></returns>
        public bool DeleteCourier(int id, string courierName)
        {
            // Output whether the queries should be sent to the database
            bool saveRequest = false;

            // Find the courier in the database
            var courier = mDBContext.Couriers.Find(id, courierName);

            if (courier == null)
                return saveRequest;

            mDBContext.Couriers.Remove(courier);

            saveRequest = true;

            return saveRequest;
        }

        /// <summary>
        /// Get all the couriers for a specific client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Couriers>> GetCouriers(int id)
        {
            List<Couriers> couriers = new List<Couriers>();
            // Get all couriers with this Id
            await Task.Run(() =>
            {
                couriers = mDBContext.Couriers.Where((c) => c.Id == id).ToList();
            });

            return couriers;
        }

        public List<Couriers> UpdateCouriers(List<Couriers> couriers)
        {
            // Find the couriers in the database
            var couriersForUpdate = mDBContext.Couriers.Where(cr => cr.Id == couriers[0].Id).ToList();

            if (couriersForUpdate == null || couriersForUpdate.Count == 0)
                return null;

            couriersForUpdate = couriers;
            return couriersForUpdate;
        }

        #endregion
    }
}
