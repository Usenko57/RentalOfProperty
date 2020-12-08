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
        [Required(ErrorMessage = "Введите город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Введите улицу")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Введите номер дома")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Введите номер квартиры")]
        public string FlatNumber { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Выберите тип дома")]
        public string TypeOfHouse { get; set; }

        [Required(ErrorMessage = "Выберите балкон")]
        public string Balcony { get; set; }

        [Required(ErrorMessage = "Введите количество комнат")]        
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Введите цену аренды")]
        public double PriceForMonth { get; set; }

        [Required(ErrorMessage = "Введите общую площадь квартиры")]
        public double TotalArea { get; set; }

        public string AdditionalInformation { get; set; }

        public int OwnerId { get; set; }

        public string FlatPicturePath { get; set; }

        [Required(ErrorMessage = "Пожалуйста выберите фотографию квартиры")]
        public IFormFile FlatPicture { get; set; }

        public User Owner { get; set; }

        public IEnumerable<Flat> Flats { get; set; }

        public string CitySearch { get; set; }

        public double? FromPrice { get; set; }
        public double? ToPrice { get; set; }

    }
}
