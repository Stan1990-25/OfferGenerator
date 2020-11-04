using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMOffersClients.ViewModels;

namespace TMOffersClients.Models
{
    public class OffersRepository : IOffersRepository
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
        public OffersRepository(AppDbContext dbContext)
        {
            mDBContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add offer to the database
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public Offers AddOffer(Offers offer, List<OrderProduct> products)
        {
            mDBContext.Offers.Add(offer);
            List<OffersPorducts> offersPorducts = new List<OffersPorducts>();

            for (int i = 0; i < products.Count(); i++)
            {
                offersPorducts.Add(new OffersPorducts
                {
                    OfferID = offer.OfferNumber,
                    ProductID = products[i].Id,
                    Quantity = products[i].Quantity,
                    Meters = products[i].Meters
                });
            }

            mDBContext.OffersPorducts.AddRange(offersPorducts);

            mDBContext.SaveChanges();

            return offer;
        }

        /// <summary>
        /// Update offer in the database
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public Offers ChangeOffer(Offers offer, List<OrderProduct> products)
        {
            var dbOffer = mDBContext.Offers.Find(offer.OfferNumber);

            if (dbOffer == null)
                return null;

            dbOffer.ExpirationDays = offer.ExpirationDays;
            dbOffer.DeliveryTerms = offer.DeliveryTerms;
            dbOffer.PaymentType = offer.PaymentType;
            dbOffer.DeliveryDeadline = offer.DeliveryDeadline;
            dbOffer.DeliveryType = offer.DeliveryType;
            dbOffer.Author = offer.Author;
            dbOffer.DateModified = DateTimeOffset.Now;
            dbOffer.OrderTotal = offer.OrderTotal;

            var currProducts = mDBContext.OffersPorducts.Where(op => op.OfferID == dbOffer.OfferNumber);
            mDBContext.OffersPorducts.RemoveRange(currProducts);

            List<OffersPorducts> offersPorducts = new List<OffersPorducts>();

            for (int i = 0; i < products.Count(); i++)
            {
                offersPorducts.Add(new OffersPorducts
                {
                    OfferID = dbOffer.OfferNumber,
                    ProductID = products[i].Id,
                    Quantity = products[i].Quantity
                });
            }

            mDBContext.OffersPorducts.AddRange(offersPorducts);

            mDBContext.SaveChanges();

            return offer;
        }

        /// <summary>
        /// Delete offer from the database
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public Offers DeleteOffer(string offerId)
        {
            var moddedId = offerId.Replace("%2F", "/");
            var dbOffer = mDBContext.Offers.Find(moddedId);

            if (dbOffer == null)
                return null;

            var offerProducts = mDBContext.OffersPorducts.Where(op => op.OfferID == moddedId).ToList();

            if (offerProducts == null || offerProducts.Count() == 0)
                return null;

            mDBContext.OffersPorducts.RemoveRange(offerProducts);
            mDBContext.Offers.Remove(dbOffer);

            mDBContext.SaveChanges();

            return dbOffer;
        }

        /// <summary>
        /// Get all offers from the database
        /// </summary>
        /// <returns></returns>
        public List<Offers> GetAllOffers()
        {
            return mDBContext.Offers.Where(off => off.OfferNumber.Contains("-01/") || off.IsApproved).ToList();
        }

        /// <summary>
        /// Get all the revisions for the offer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Offers> GettAllOfferRevisions(string id)
        {
            return mDBContext.Offers.Where(off => off.OfferNumber.Contains(id.Substring(0, 10))).ToList();
        }

        /// <summary>
        /// Get all the products for a single offer
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public List<OffersPorducts> GetAllProductsForOffer(string offerId)
        {
            return mDBContext.OffersPorducts.Where(op => op.OfferID == offerId).ToList();
        }

        /// <summary>
        /// Get a single offer
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public Offers GetOffer(string offerId)
        {
            var offer = mDBContext.Offers.Find(offerId);

            if (offer == null)
                return null;

            return offer;
        }

        /// <summary>
        /// Get the last offer from the database
        /// </summary>
        /// <returns></returns>
        public string GetLastOffer()
        {
            var lastOffer = mDBContext.Offers.ToList();
            if (lastOffer.Count > 0)
                return lastOffer[lastOffer.Count - 1].OfferNumber;
            else
                return null;
        }

        /// <summary>
        /// Approve offer
        /// </summary>
        /// <param name="id"></param>
        public async Task ApproveOffer(string id)
        {
            var offer = await mDBContext.Offers.FindAsync(id);

            offer.IsApproved = true;

            mDBContext.Offers.Update(offer);

            await mDBContext.SaveChangesAsync();
        }

        /// <summary>
        /// Approve offer
        /// </summary>
        /// <param name="id"></param>
        public async Task DisApproveOffer(string id)
        {
            var offer = await mDBContext.Offers.FindAsync(id);

            offer.IsApproved = false;

            mDBContext.Offers.Update(offer);

            await mDBContext.SaveChangesAsync();
        }

        #endregion
    }
}
