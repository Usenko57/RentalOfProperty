using RentalOfProperty.Models;
using RentalOfProperty.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalOfProperty.Data
{
    public interface IDataRepository
    {
        IEnumerable<Flat> GetFlats();
        Flat GetFlat(int flat_id);
        Address GetAddress(int address_id);
        Street GetStreet(int street_id);
        City GetCity(int city_id);
        User LoginUser(string email, string password);

        User GetUserByEmail(string email);

        void InsertFlat(FlatViewModel model);
        void RegisterUser(RegisterModel model);
    }
}
