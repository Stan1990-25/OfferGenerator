using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TMOffersClients
{
    public class ListClientsViewModel
    {
        #region Public properties

        /// <summary>
        /// The clients
        /// </summary>
        public List<ClientViewModel> Clients { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ListClientsViewModel()
        {
            Clients = new List<ClientViewModel>();
        }

        #endregion
    }

    public class ClientViewModel
    {
        #region Public properties

        /// <summary>
        /// The client
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// The clients contacts
        /// </summary>
        public IList<ContactPerson> Contacts { get; set; }

        /// <summary>
        /// The clients delivery addresses
        /// </summary>
        public IList<DeliveryAddresses> DeliveryAddresses { get; set; }

        /// <summary>
        /// The clients couriers
        /// </summary>
        public IList<Couriers> Couriers { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientViewModel()
        {
            Client = new Client();
            Contacts = new List<ContactPerson>();
            DeliveryAddresses = new List<DeliveryAddresses>();
            Couriers = new List<Couriers>();
        }

        #endregion

    }
}
