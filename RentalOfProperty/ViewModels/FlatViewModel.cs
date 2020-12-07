using Microsoft.AspNetCore.Http;
using RentalOfProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalOfProperty.ViewModels
{
    public class FlatViewModel
    {
        public IEnumerable<Flat> Flats { get; set; }
        public IFormFile FlatPicture { get; set; }
    }
}
