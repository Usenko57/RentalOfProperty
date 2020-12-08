using Microsoft.AspNetCore.Http;
using RentalOfProperty.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentalOfProperty.ViewModels
{
    public class FlatViewModel
    {
        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string FlatNumber { get; set; }

        public string Header { get; set; }

        public string TypeOfHouse { get; set; }

        public string Balcony { get; set; }

        public int NumberOfRooms { get; set; }

        public double PriceForMonth { get; set; }

        public double TotalArea { get; set; }

        public string AdditionalInformation { get; set; }

        public int OwnerId { get; set; }

        public string FlatPicturePath { get; set; }

        [Required(ErrorMessage = "Please choose profile image")]
        public IFormFile FlatPicture { get; set; }

        public User Owner { get; set; }

        public IEnumerable<Flat> Flats { get; set; }
    }
}
