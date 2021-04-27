using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSearchWebApplication.Models
{
    public class HotelInfo
    {
        public string City { get; set; }
        public DateTime CheckInDateRequest { get; set; }
        public DateTime CheckOutDateRequest { get; set; }
        public byte NumberOfPassengersForOneRoom { get; set; }
        public bool IsHotelAvailable { get; set; }

        //svi pozivi prema serverskoj strani trebaju biti async await (Ajax)
        //direktno se spojiti na AmadeusApi

    }
}
