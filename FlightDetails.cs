using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManagement
{
    public class FlightDetails
    {
        public int flightNumber { get; set; }
        public int flightCapacity { get; set; }
        public string sourceCity { get; set; }
        public string sourceCityCode { get; set; }
        public string destinationCity { get; set; }
        public string destinationCityCode { get; set; }
    }
}
