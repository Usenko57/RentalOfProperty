using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalOfProperty.Models
{
    public class Flat
    {
        public int Id { get; set; }
        public int AddressId { get; set; }

        public string Header { get; set; }

        public string TypeOfHouse { get; set; }

        public string Balcony { get; set; }

        public int NumberOfRooms { get; set; }

        public double PriceForMonth { get; set; }

        public double TotalArea { get; set; }

        public string AdditionalInformation { get; set; }

        public string FlatPictures { get; set; }

        public Address Address { get; set; }
    }
}
