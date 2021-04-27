using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotelSearchWebApplication.Models;
using System.Configuration;
using Afonsoft.Amadeus;
using Afonsoft.Amadeus.Resources;
using HotelSearchWebApplication.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Afonsoft.Amadeus.Client;
using System.IO;
using System.Net.Http.Headers;
using static HotelSearchWebApplication.Models.ViewModel.HotelViewModel;
using System.Globalization;

namespace HotelSearchWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateABooking()
        {
            return View();
        }

        [ResponseCache(Duration = 20)]
        public async Task<IActionResult> CheckAvailability(string City, DateTime CheckInDateRequest, DateTime CheckOutDateRequest, byte NumberOfPassengersForOneRoom)
        {
            List<ReturnHotelInformationViewModel> returnHotelInfo = new List<ReturnHotelInformationViewModel>();

            try
            {
                var apiKey = _config.GetValue<string>("ApiKey");
                var apiSecret = _config.GetValue<string>("ApiSecret");

                Amadeus amadeus = Amadeus.Builder(apiKey, apiSecret).Build();
                string token = await GetToken(apiKey, apiSecret, amadeus);

                const string baseUrl = "https://test.api.amadeus.com/v1/";
                const string dateFormat = "yyyy-MM-dd";
                string city = FilterCity(City);

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await ConnectToAWebsite(token, baseUrl, city, client);
                    if (response.IsSuccessStatusCode)
                    {
                        var readAllAsync = await response.Content.ReadAsStringAsync();
                        var hotelViewModel = JsonConvert.DeserializeObject<Root>(readAllAsync);
                        MapAndSaveHotels(CheckInDateRequest, CheckOutDateRequest, NumberOfPassengersForOneRoom, returnHotelInfo, dateFormat, hotelViewModel);
                    }
                }
                ViewBag.HotelViewModel = returnHotelInfo;
            }
            catch (Exception e)
            {
                //Also log with logger
                Console.WriteLine("ERROR: " + e.ToString());
            }
            return View(returnHotelInfo);
        }

        private static void MapAndSaveHotels(DateTime CheckInDateRequest, DateTime CheckOutDateRequest, byte NumberOfPassengersForOneRoom, List<ReturnHotelInformationViewModel> returnHotelInfo, string dateFormat, Root hotelViewModel)
        {
            foreach (var data in hotelViewModel.data)
            {
                ReturnHotelInformationViewModel returnInfo;
                if (data.available)
                {
                    foreach (var off in data.offers)
                    {
                        DateTime checkInDate, checkOutDate;
                        ParseDates(dateFormat, off, out checkInDate, out checkOutDate);

                        if (checkInDate > CheckInDateRequest && checkOutDate < CheckOutDateRequest && NumberOfPassengersForOneRoom <= off.guests.adults)
                        {
                            returnInfo = new ReturnHotelInformationViewModel()
                            {
                                NameOfHotel = data.hotel.name,
                                NumberOfRatings = data.hotel.rating,
                                Description = (data.hotel.description == null) ? "This hotel has no description" : data.hotel.description.text,
                                PriceForRoom = off.price.total,
                                CurrencyForPayment = off.price.currency
                            };
                            returnHotelInfo.Add(returnInfo);
                        }
                    }
                }
                else
                    returnInfo = SaveANewRecordWithoutAvailableRooms(returnHotelInfo, data);
            }
        }

        private static void ParseDates(string dateFormat, Offer off, out DateTime checkInDate, out DateTime checkOutDate)
        {
            checkInDate = DateTime.ParseExact(off.checkInDate, dateFormat, CultureInfo.InvariantCulture);
            checkOutDate = DateTime.ParseExact(off.checkOutDate, dateFormat, CultureInfo.InvariantCulture);
        }

        private static async Task<HttpResponseMessage> ConnectToAWebsite(string token, string baseUrl, string city, HttpClient client)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string siteToSee = "https://test.api.amadeus.com/v2/shopping/hotel-offers";
            if (!String.IsNullOrEmpty(city))
                siteToSee = "https://test.api.amadeus.com/v2/shopping/hotel-offers?cityCode=" + city;

            HttpResponseMessage response = await client.GetAsync(siteToSee);
            return response;
        }

        private static ReturnHotelInformationViewModel SaveANewRecordWithoutAvailableRooms(List<ReturnHotelInformationViewModel> returnHotelInfo, Datum data)
        {
            ReturnHotelInformationViewModel returnInfo = new ReturnHotelInformationViewModel()
            {
                NameOfHotel = data.hotel.name,
                NumberOfRatings = data.hotel.rating,
                Description = data.hotel.description.ToString(),
            };
            returnHotelInfo.Add(returnInfo);
            return returnInfo;
        }

        private string FilterCity(string city)
        {
            if (city != null)
                return city.Substring(0, 3).ToUpper();
            return String.Empty;
        }

        private async Task<string> GetToken(string client_id, string client_secret, Amadeus amadeus)
        {
            string s = null;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://test.api.amadeus.com/v1/security/oauth2/token"))
                {
                    request.Content = new StringContent("grant_type=client_credentials&client_id=" + client_id
                        + "&client_secret=" + client_secret, Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        s = await response.Content.ReadAsStringAsync();
                    }
                }
            }

            string returnToken = s.Split(',')[5].Substring(30).TrimEnd('"');
            return returnToken;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
