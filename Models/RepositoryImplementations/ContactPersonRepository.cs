using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMOffersClients.Models
{
    /// <summary>
    /// The class for the DI representing the <see cref="ContactPerson"/> class
    /// </summary>
    public class ContactPersonRepository : IContactPersonRepository
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
        public ContactPersonRepository(AppDbContext dbContext)
        {
            mDBContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a contact person to the database
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public bool AddContact(ContactPerson contact)
        {
            mDBContext.ContactPersons.Add(contact);
            return true;
        }

        /// <summary>
        /// Delete contact from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteContact(int id, string name)
        {
            // Output whether the queries should be sent to the database
            bool saveRequest = false;

            // Find the contact in the database
            var contact = mDBContext.ContactPersons.Find(id, name);

            if (contact == null)
                return saveRequest;

            mDBContext.ContactPersons.Remove(contact);

            saveRequest = true;

            return saveRequest;
        }

        /// <summary>
        /// Get single contact by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ContactPerson GetContact(string name)
        {
            var contact = mDBContext.ContactPersons.First(cp => cp.Name == name);

            if (contact == null)
                return null;

            return contact;
        }

        /// <summary>
        /// Get all contacts via Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContactPerson>> GetContacts(int id)
        {
            List<ContactPerson> contacts = new List<ContactPerson>();
            // Get all contacts with this Id
            await Task.Run(() =>
            {
                contacts = mDBContext.ContactPersons.Where(cp => cp.Id == id).ToList();
            });

            return contacts;
        }

        /// <summary>
        /// Update a contact
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public List<ContactPerson> UpdateContacts(List<ContactPerson> contacts)
        {
            // Find the contacts in the database
            var contactPersons = mDBContext.ContactPersons.Where(cp => cp.Id == contacts[0].Id).ToList();

            if (contactPersons == null || contactPersons.Count == 0)
                return null;

            contactPersons = contacts;
            return contactPersons;
        }

        #endregion
    }
}
