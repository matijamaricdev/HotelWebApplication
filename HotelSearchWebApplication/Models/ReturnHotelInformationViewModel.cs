using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSearchWebApplication.Models
{
    public class ReturnHotelInformationViewModel
    {
        public string NameOfHotel { get; set; }
        public string NumberOfRatings { get; set; }
        public string Description { get; set; }
        public string PriceForRoom { get; set; }
        public string CurrencyForPayment { get; set; }
    }
}
