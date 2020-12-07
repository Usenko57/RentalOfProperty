using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalOfProperty.ViewModels
{
    public class FlatViewModel
    {
        public int Id { get; set; }
        public string City { get; set; }

        public string Street { get; set; }

        public int HouseNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }

        public string TypeOfHouse { get; set; }

        public string Balcony { get; set; }

        public int CountOfRoom { get; set; }

        public float PriceForMonth { get; set; }

        public float TotalArea { get; set; }

        public string AdditionalInformation { get; set; }

        public IFormFile FlatPicture { get; set; }
    }
}
