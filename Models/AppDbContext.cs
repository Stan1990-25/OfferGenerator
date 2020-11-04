using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMOffersClients.Models;

namespace TMOffersClients
{
    /// <summary>
    /// The Database context class
    /// </summary>
    public class AppDbContext : IdentityDbContext<TMUser>
    {

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #endregion

        #region Public properties

        // The tables in the database

        public DbSet<Client> Clients { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<Couriers> Couriers { get; set; }
        public DbSet<DeliveryAddresses> DeliveryAddresses { get; set; }
        public DbSet<Offers> Offers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PaymentsFromClients> Payments { get; set; }
        public DbSet<OffersPorducts> OffersPorducts { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Setting the combined primary keys for 
        /// <see cref="ContactPersons"/>, <see cref="Couriers"/>, <see cref="DeliveryAddresses"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ContactPerson>(cp => cp.HasKey(cpp => new { cpp.Id, cpp.Name }));
            modelBuilder.Entity<Couriers>(cour => cour.HasKey(c => new { c.Id, c.CourierName}));
            modelBuilder.Entity<DeliveryAddresses>(dadd => dadd.HasKey(da => new { da.Id, da.DelAddress }));
            modelBuilder.Entity<OffersPorducts>(ops => ops.HasKey(op => new { op.OfferID, op.ProductID, op.Meters }));
        }

        #endregion
    }
}
