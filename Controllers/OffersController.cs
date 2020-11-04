using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TMOffersClients.HelperClasses;
using TMOffersClients.Models;
using TMOffersClients.ViewModels;

namespace TMOffersClients.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {

        #region Private members

        /// <summary>
        /// Injecting the <see cref="ClientRepository"/> class
        /// </summary>
        private readonly IClientRepository mClientRepo;

        private readonly IContactPersonRepository mContactPersons;

        private readonly ICouriersRepository mCouriers;
        private readonly IDeliveryRepository mDeliveryAddresses;
        /// <summary>
        /// Injecting the <see cref="ProductRepository"/> class on every request
        /// </summary>
        private readonly IProductRepository mProductRepository;

        private readonly IOffersRepository mOffersRepository;

        private static List<Offers> allOffers;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public OffersController(IClientRepository clientRepository, IContactPersonRepository contacts,
                                ICouriersRepository couriers, IDeliveryRepository deliveryAddresses, 
                                IProductRepository productRepository, IOffersRepository offersRepository)
        {
            mClientRepo = clientRepository;
            mContactPersons = contacts;
            mCouriers = couriers;
            mDeliveryAddresses = deliveryAddresses;
            mProductRepository = productRepository;
            mOffersRepository = offersRepository;
        }

        #endregion

        #region Controller Methods

        /// <summary>
        /// Get the List Offers view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ListOffers()
        {
            List<OfferViewModel> offers = new List<OfferViewModel>();

            allOffers = mOffersRepository.GetAllOffers();

            for (int i = 0; i < allOffers.Count; i++)
            {
                offers.Add(await MapDbOfferObjectToViewModelObject(allOffers[i]));
            }

            return View(new ListOffersViewModel
            {
                Offers = offers,
                AllClients = await GetAllClients()
            });
        }

        /// <summary>
        /// Get only filtered offers by client name or date
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Filter(DateTimeOffset fromDate, DateTimeOffset toDate, string searchText = null)
        {
            if (string.IsNullOrEmpty(searchText) && fromDate == default && toDate == default)
                return RedirectToAction(nameof(ListOffers));

            ListOffersViewModel model = new ListOffersViewModel();
            model.AllClients = await GetAllClients();

            if (!string.IsNullOrEmpty(searchText) || fromDate != default || toDate != default)
            {
                var clIndex = -1;
                if (!string.IsNullOrEmpty(searchText))
                    clIndex = model.AllClients.Clients.FindIndex(cl => StringHelpers.Contains(cl.Client.Name, searchText, StringComparison.OrdinalIgnoreCase));


                List<Offers> offersByDate;
                if (clIndex == -1)
                    offersByDate = allOffers.Where(off => fromDate.CompareTo(off.DateModified) <= 0 || toDate.CompareTo(off.DateModified) >= 0).ToList();
                else if (clIndex > -1 && fromDate == default && toDate == default)
                    offersByDate = allOffers.Where(off => off.ClientId == model.AllClients.Clients[clIndex].Client.Id).ToList();
                else
                {
                    offersByDate = allOffers.Where(off => off.ClientId == model.AllClients.Clients[clIndex].Client.Id && (fromDate.CompareTo(off.DateModified) <= 0
                                                            || toDate.CompareTo(off.DateModified) >= 0)).ToList();
                }

                for (int i = 0; i < offersByDate.Count; i++)
                {
                    model.Offers.Add(await MapDbOfferObjectToViewModelObject(offersByDate[i]));
                }
            }

            return View(nameof(ListOffers), model);
        }

        /// <summary>
        /// Get the "Create Offer" view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateOffer()
        {
            OfferViewModel model = new OfferViewModel();

            var lastOffer = GetTheLastOfferNumber();

            if (lastOffer == null)
                model.Number = $"ВН{DateTimeOffset.Now.ToString("yy")}-0039-01/{DateTimeOffset.Now.ToString("dd.MM.yyyy")}";
            else
                model.Number = lastOffer;

            model.AllClients = await GetAllClients();
            model.AllProducts = GetAllProducts();

            return View(model);
        }

        /// <summary>
        /// Add an offer to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost] 
        public JsonResult AddOffer([FromBody]OfferViewModel model)
        {
            model.SelectedProducts.RemoveAt(0);

            var summerizedProducts = AddUpProducts(model.SelectedProducts);

            var offerNumber = GetTheLastOfferNumber();
            if (offerNumber != null)
                model.Number = offerNumber;

            var newOffer = MapOfferViewModelToDbObject(model);

            mOffersRepository.AddOffer(newOffer, summerizedProducts);

            GeneratePDFOffer.CreatePDF(model, Startup.Environment);

            allOffers = mOffersRepository.GetAllOffers();

            return Json(new { newUrl = Url.Action("ListOffers", "Offers") });
        }

        /// <summary>
        /// Get the "SeeAllRevisions" view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SeeAllRevisions(string id)
        {
            var moddedId = id.Replace("%2F", "/");

            List<OfferViewModel> offers = new List<OfferViewModel>();

            allOffers = mOffersRepository.GettAllOfferRevisions(id);

            for (int i = 0; i < allOffers.Count; i++)
            {
                offers.Add(await MapDbOfferObjectToViewModelObject(allOffers[i]));
            }

            ViewBag.OfferId = moddedId;
            ViewBag.CurrRevNumber = offers[offers.Count - 1].Number;
            ViewBag.NextRevNumber = RevisionNumberUpdate(offers[offers.Count - 1].Number);

            return View(new ListOffersViewModel
            {
                Offers = offers,
                AllClients = await GetAllClients()
            });
        }

        /// <summary>
        /// Get the Update Offer view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateOffer(string id) // %F means "/"
        {
            var moddedId = id.Replace("%2F", "/");

            var offer = mOffersRepository.GetOffer(moddedId);

            var model = await MapDbOfferObjectToViewModelObject(offer);

            model.Number = RevisionNumberUpdate(offer.OfferNumber);

            model.AllClients = await GetAllClients();
            model.AllProducts = GetAllProducts();

            return View(model); ;
        }

        /// <summary>
        /// Add an offer to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //public JsonResult UpdateOfferPost(string number, string clientName, string clientCity, string clientPhoneNumber, int clientId, double disount, string contactName,
        //                                    List<OrderProduct> selectedProducts, 
        //                                    int expirationDays, string deliveryTerms, string paymentType, string deliveryDeadline, string deliveryType, 
        //                                    decimal total, bool isApproved)
        public JsonResult UpdateOfferPost([FromBody]OfferViewModel model)
        {
            model.SelectedProducts.RemoveAt(0);

            var summerizedProducts = AddUpProducts(model.SelectedProducts);
            model.SelectedProducts = summerizedProducts;

            model.Number = RevisionNumberUpdate(model.Number);

            var newOffer = MapOfferViewModelToDbObject(model);

            mOffersRepository.AddOffer(newOffer, summerizedProducts);

            GeneratePDFOffer.CreatePDF(model, Startup.Environment);

            allOffers = mOffersRepository.GetAllOffers();

            var originalId = model.Number.Replace("%2F", "/");
            originalId = originalId.Replace(originalId.Substring(10, 3), "01/");
            return Json(new { newUrl = Url.Action("SeeAllRevisions", "Offers", new { id = originalId }) });
        }

        /// <summary>
        /// Delete offer or revision
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteOffer(string id)
        {
            var offerToDelete = mOffersRepository.DeleteOffer(id);

            var moddedId = id.Replace("%2F", "/");

            if (moddedId.Contains("-01/"))
                return RedirectToAction(nameof(ListOffers));

            var originalId = id.Replace(id.Substring(10, 5), "01/");

            return RedirectToAction("SeeAllRevisions", new { id = originalId });
        }

        /// <summary>
        /// Approve offer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ApproveOffer(string id)
        {
            var moddedId = id.Replace("%2F", "/");
            await mOffersRepository.ApproveOffer(moddedId);

            var originaId = moddedId.Replace(moddedId.Substring(10, 3), "01/");
            return RedirectToAction("SeeAllRevisions", new { id = originaId });
        }

        /// <summary>
        /// DisApprove offer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DisApproveOffer(string id)
        {
            var moddedId = id.Replace("%2F", "/");
            await mOffersRepository.DisApproveOffer(moddedId);

            var originaId = moddedId.Replace(moddedId.Substring(10, 3), "01/");
            return RedirectToAction("SeeAllRevisions", new { id = originaId });
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get all clients for the IntelliSense
        /// </summary>
        /// <returns></returns>
        public async Task<ListClientsViewModel> GetAllClients()
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
            return listContacts;
        }

        /// <summary>
        /// Get all products for IntelliSense
        /// </summary>
        /// <returns></returns>
        private List<Product> GetAllProducts()
        {
            var products = mProductRepository.GetAllProducts().ToList();
            return products;
        }

        /// <summary>
        /// Summerizing all the products
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private List<OrderProduct> AddUpProducts(List<OrderProduct> products)
        {
            products.Sort((x, y) => x.Id.CompareTo(y.Id));
            List<OrderProduct> summedProducts = new List<OrderProduct>();
            for (int i = 0; i < products.Count; i++)
            {
                if (i == 0)
                    summedProducts.Add(products[i]);
                else
                {
                    var searchIndex = summedProducts.FindIndex(pr => pr.Id == products[i].Id && pr.Meters == products[i].Meters);
                    if (searchIndex == -1)
                        summedProducts.Add(products[i]);
                    else
                        summedProducts[searchIndex].Quantity += products[i].Quantity;
                }
            }

            return summedProducts;
        }

        /// <summary>
        /// Map the <see cref="OfferViewModel"/> object to <see cref="Offers"/> database object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Offers MapOfferViewModelToDbObject(OfferViewModel model)
        {
            return new Offers
            {
                OfferNumber = model.Number,
                ClientId = model.ClientId,
                ContactName = model.ContactName,
                Discount = model.Discount,
                ExpirationDays = model.ExpirationDays,
                DeliveryTerms = model.DeliveryTerms,
                PaymentType = model.PaymentType,
                DeliveryDeadline = model.DeliveryDeadline,
                DeliveryType = model.DeliveryType,
                Author = model.Author,
                DateModified = DateTimeOffset.Now,
                OrderTotal = model.Total
            };
        }

        /// <summary>
        /// Map the <see cref="Offers"/> database object to <see cref="OfferViewModel"/> view model object
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        private async Task<OfferViewModel> MapDbOfferObjectToViewModelObject(Offers offer)
        {
            var client = mClientRepo.GetClient(offer.ClientId);
            var contact = mContactPersons.GetContact(offer.ContactName);

            var products = mOffersRepository.GetAllProductsForOffer(offer.OfferNumber);
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            foreach (var product in products)
            {
                var dbProduct = mProductRepository.GetProductById(product.ProductID);
                var orderProduct = MapDbProductToViewModelProduct(dbProduct, product.Quantity, product.Meters);
                orderProducts.Add(orderProduct);
            }

            return new OfferViewModel
            {
                Number = offer.OfferNumber,
                ClientId = client.Id,
                ClientName = client.Name,
                ClientCity = client.City,
                ClientPhoneNumber = contact.PhoneNumber,
                Discount = offer.Discount,
                ContactName = offer.ContactName,
                SelectedProducts = orderProducts,
                ExpirationDays = offer.ExpirationDays,
                DeliveryTerms = offer.DeliveryTerms,
                PaymentType = offer.PaymentType,
                DeliveryDeadline = offer.DeliveryDeadline,
                DeliveryType = offer.DeliveryType,
                Total = offer.OrderTotal,
                Author = offer.Author,
                DateModified = offer.DateModified,
                IsApproved = offer.IsApproved,
                AllProducts = GetAllProducts(),
                AllClients = await GetAllClients()
            };
        }

        /// <summary>
        /// Map <see cref="Product"/> database object to <see cref="OrderProduct"/> object
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private OrderProduct MapDbProductToViewModelProduct(Product product, double quantity, double meters = 0.0)
        {
            return new OrderProduct
            {
                Id = product.Id,
                ProductNumber = product.OrderNumber,
                Description = product.Description,
                Category = product.Category,
                Quantity = quantity,
                Meters = meters,
                UnitPrice = product.Price
            };
        }

        /// <summary>
        /// Returns the new revision number of an offer
        /// </summary>
        /// <param name="currOfferRevNo"></param>
        /// <returns></returns>
        private string RevisionNumberUpdate(string currOfferRevNo)
        {
            int currRevNumber;
            int.TryParse(currOfferRevNo.Split('/')[0].Substring(currOfferRevNo.Split('/')[0].Length - 2), out currRevNumber);

            currRevNumber++;

            var firstPartOfOfferNoLength = currOfferRevNo.Split('/')[0].Length;

            return currOfferRevNo.Replace(currOfferRevNo.Split('/')[0].Substring(firstPartOfOfferNoLength - 2) + "/",
                                                    currRevNumber.ToString().Length == 1 ? "0" + currRevNumber.ToString() + "/" : currRevNumber.ToString() + "/");
        }

        /// <summary>
        /// Get the last offer number
        /// </summary>
        /// <returns></returns>
        private string GetTheLastOfferNumber()
        {
            var lastOffer = mOffersRepository.GetLastOffer();
            if (lastOffer == null)
                return null;
            else
            {
                var offerNumberStartIndex = lastOffer.IndexOf('-');
                var offerNumber = lastOffer.Substring(offerNumberStartIndex + 1, 4);
                var newOfferNumber = (int.Parse(offerNumber) + 1).ToString();

                while (newOfferNumber.Length < 4)
                {
                    newOfferNumber = newOfferNumber.Insert(0, "0");
                }

                return $"ВН{DateTimeOffset.Now.ToString("yy")}-{newOfferNumber}-01/{DateTimeOffset.Now.ToString("dd.MM.yyyy")}";
            }
        }

        #endregion
    }
}
