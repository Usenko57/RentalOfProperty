using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalOfProperty.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int StreetId { get; set; }
        public string HouseNumber { get; set; }
        
        public string FlatNumber { get; set; }

        public City City { get; set; }

        public Street Street { get; set; }
    }
}
