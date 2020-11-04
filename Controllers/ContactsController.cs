using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMOffersClients.Models;

namespace TMOffersClients
{
    [Authorize]
    public class ContactsController : Controller
    {
        #region Private members

        /// <summary>
        /// Injecting the <see cref="ClientRepository"/> class
        /// </summary>
        private readonly IClientRepository mClientRepo;

        private readonly IContactPersonRepository mContactPersons;

        private readonly ICouriersRepository mCouriers;
        private readonly IDeliveryRepository mDeliveryAddresses;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactsController(IClientRepository clientRepository, IContactPersonRepository contacts,
                                ICouriersRepository couriers, IDeliveryRepository deliveryAddresses)
        {
            mClientRepo = clientRepository;
            mContactPersons = contacts;
            mCouriers = couriers;
            mDeliveryAddresses = deliveryAddresses;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all contacts from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListContacts()
        {
            var listContacts = new ListClientsViewModel();

            // Get the clients
            List<Client> clients = mClientRepo.GetAllClients();

            // Get the clients contact persons, delivery addresses and couriers
            for (int i = 0; i < clients.Count; i++)
            {
                // Get the contacts
                var contacts = await mContactPersons.GetContacts(clients[i].Id);
                // Get the delivery addresses
                var addresses = await mDeliveryAddresses.GetAddresses(clients[i].Id);
                // Get the couriers
                var couriers = await mCouriers.GetCouriers(clients[i].Id);

                //Update the view model
                ClientViewModel clientViewModel = new ClientViewModel
                {
                    Client = clients[i],
                    Contacts = contacts.ToList(),
                    DeliveryAddresses = addresses.ToList(),
                    Couriers = couriers.ToList()
                };

                listContacts.Clients.Add(clientViewModel);
            }
            return View(listContacts);
        }

        /// <summary>
        /// Get the view for adding contact
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddContact()
        {
            ClientViewModel clientViewModel = new ClientViewModel();
            return View(clientViewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddContact(ClientViewModel model)
        {
            bool clientAddingSucc = false;

            // Check whether client with specified name already exists
            if (mClientRepo.GetClient(model.Client.Name) != null)
            {
                return Json(new { newUrl = Url.Action("OpenMessageView", new { message = "Клиент с това име вече съществува" }) });
            }

            clientAddingSucc = mClientRepo.AddClient(model.Client, model.Contacts.ToList(), model.DeliveryAddresses.ToList(), model.Couriers.ToList());

            return Json(new { newUrl = Url.Action("ListContacts", "Contacts") });
        }

        /// <summary>
        /// Getting the UpdateClient view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateClient(int id)
        {
            ClientViewModel clientSelected = new ClientViewModel();

            clientSelected.Client = mClientRepo.GetClient(id);
            var contacts = await mContactPersons.GetContacts(id);
            var deliveryAddresses = await mDeliveryAddresses.GetAddresses(id);
            var couriers = await mCouriers.GetCouriers(id);

            clientSelected.Contacts = contacts.ToList();
            clientSelected.DeliveryAddresses = deliveryAddresses.ToList();
            clientSelected.Couriers = couriers.ToList();

            return View(clientSelected);
        }

        /// <summary>
        /// Updating the information about the client
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateClient(ClientViewModel model)
        {
            if (mClientRepo.UpdateClient(model.Client, model.Contacts.ToList(), model.DeliveryAddresses.ToList(), model.Couriers.ToList()))
                return Json(new { newUrl = Url.Action("ListContacts", "Contacts") });
            else
            {
                return Json(new { newUrl = Url.Action("OpenMessageView", new { message = "Проблем при промяната на данните за клиент \"" + model.Client.Name + "\"" }) });
            }
        }

        /// <summary>
        /// Retrun the view for Delete client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteClient(int id)
        {
            ClientViewModel clientSelected = new ClientViewModel();

            clientSelected.Client = mClientRepo.GetClient(id);
            var contacts = await mContactPersons.GetContacts(id);
            var deliveryAddresses = await mDeliveryAddresses.GetAddresses(id);
            var couriers = await mCouriers.GetCouriers(id);

            clientSelected.Contacts = contacts.ToList();
            clientSelected.DeliveryAddresses = deliveryAddresses.ToList();
            clientSelected.Couriers = couriers.ToList();

            return View(clientSelected);
        }

        /// <summary>
        /// Deleting the client from the DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteClient(int id, ClientViewModel clientViewModel)
        {
            if (mClientRepo.DeleteClient(id))
                return RedirectToAction(nameof(ListContacts));
            else
                return RedirectToAction("Message", new { message = "Неуспешно изтриване на клиент!" });
        }

        /// <summary>
        /// Search clients by name
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SearchClient(string searchText)
        {

            if (string.IsNullOrEmpty(searchText))
                return RedirectToAction(nameof(ListContacts));

            var listContacts = new ListClientsViewModel();

            var result = mClientRepo.SearchClient(searchText);

            for (int i = 0; i < result.Count; i++)
            {
                // Get the contacts
                var contacts = await mContactPersons.GetContacts(result[i].Id);
                // Get the delivery addresses
                var addresses = await mDeliveryAddresses.GetAddresses(result[i].Id);
                // Get the couriers
                var couriers = await mCouriers.GetCouriers(result[i].Id);

                //Update the view model
                ClientViewModel clientViewModel = new ClientViewModel
                {
                    Client = result[i],
                    Contacts = contacts.ToList(),
                    DeliveryAddresses = addresses.ToList(),
                    Couriers = couriers.ToList()
                };

                listContacts.Clients.Add(clientViewModel);
            }

            return View(nameof(ListContacts), listContacts);
        }

        /// <summary>
        /// Open message view
        /// </summary>
        /// <returns></returns>
        public IActionResult OpenMessageView(string message)
        {
            return View("Message", message);
        }

        #endregion
    }
}