using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSearchWebApplication.Models.ViewModel
{
    public class HotelViewModel
    {
        public class HotelDistance
        {
            public double distance { get; set; }
            public string distanceUnit { get; set; }
        }

        public class Address
        {
            public List<string> lines { get; set; }
            public string postalCode { get; set; }
            public string cityName { get; set; }
            public string countryCode { get; set; }
        }

        public class Contact
        {
            public string phone { get; set; }
            public string fax { get; set; }
            public string email { get; set; }
        }

        public class Description
        {
            public string lang { get; set; }
            public string text { get; set; }
        }

        public class Medium
        {
            public string uri { get; set; }
            public string category { get; set; }
        }

        public class Hotel
        {
            public string type { get; set; }
            public string hotelId { get; set; }
            public string chainCode { get; set; }
            public string dupeId { get; set; }
            public string name { get; set; }
            public string rating { get; set; }
            public string cityCode { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public HotelDistance hotelDistance { get; set; }
            public Address address { get; set; }
            public Contact contact { get; set; }
            public Description description { get; set; }
            public List<string> amenities { get; set; }
            public List<Medium> media { get; set; }
        }

        public class RateFamilyEstimated
        {
            public string code { get; set; }
            public string type { get; set; }
        }

        public class TypeEstimated
        {
            public string category { get; set; }
            public int beds { get; set; }
            public string bedType { get; set; }
        }

        public class Room
        {
            public string type { get; set; }
            public TypeEstimated typeEstimated { get; set; }
            public Description description { get; set; }
        }

        public class Guests
        {
            public int adults { get; set; }
        }

        public class Average
        {
            public string @base { get; set; }
        }

        public class Change
        {
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string @base { get; set; }
            public string total { get; set; }
        }

        public class Variations
        {
            public Average average { get; set; }
            public List<Change> changes { get; set; }
        }

        public class Price
        {
            public string currency { get; set; }
            public string @base { get; set; }
            public string total { get; set; }
            public Variations variations { get; set; }
        }

        public class AcceptedPayments
        {
            public List<string> creditCards { get; set; }
            public List<string> methods { get; set; }
        }

        public class Guarantee
        {
            public AcceptedPayments acceptedPayments { get; set; }
        }

        public class Cancellation
        {
            public int numberOfNights { get; set; }
            public DateTime deadline { get; set; }
        }

        public class HoldTime
        {
            public DateTime deadline { get; set; }
        }

        public class Policies
        {
            public Guarantee guarantee { get; set; }
            public string paymentType { get; set; }
            public Cancellation cancellation { get; set; }
            public HoldTime holdTime { get; set; }
        }

        public class Commission
        {
            public string amount { get; set; }
            public string percentage { get; set; }
        }

        public class Offer
        {
            public string id { get; set; }
            public string checkInDate { get; set; }
            public string checkOutDate { get; set; }
            public string rateCode { get; set; }
            public RateFamilyEstimated rateFamilyEstimated { get; set; }
            public Room room { get; set; }
            public Guests guests { get; set; }
            public Price price { get; set; }
            public Policies policies { get; set; }
            public Commission commission { get; set; }
        }

        public class Datum
        {
            public string type { get; set; }
            public Hotel hotel { get; set; }
            public bool available { get; set; }
            public List<Offer> offers { get; set; }
            public string self { get; set; }
        }

        public class Links
        {
            public string next { get; set; }
        }

        public class Meta
        {
            public Links links { get; set; }
        }

        public class Root
        {
            public List<Datum> data { get; set; }
            public Meta meta { get; set; }
        }
    }
}
