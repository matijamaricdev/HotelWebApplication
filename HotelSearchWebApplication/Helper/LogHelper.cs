using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelSearchWebApplication.Helper
{
    public class LogHelper
    {
        public const string UserTriedToMakeRequest = "User tried to make a request.";
        public const string CredentialsAreEmpty = "Please, check your settings. Value for ApiKey or ApiSecret is empty. You cannot enter the application without credentials.";
        public const string TokenSuccessfullySent = "Token has been successfully sent.";
        public const string WrongCityFiltering = "I can't filter a city, it doesn't have a value. Entered city";
        public const string HotelDoesNotHaveDescription = "This hotel has no description";
        public const string SuccessfullyConnected = "Successfully connected to a website.";
        public const string SuccessfullyParsedJSON = "Successfully parsed JSON from client.";
        public const string ErrorMessage = "Something bad happened. More information about exception is here.";
        public const string SuccessfullyShowedHotel = "Successfully parsed and showed hotel on view.";
        public const string CurrentlyNoHotels = "There are no hotels for desired city.";
    }
}
