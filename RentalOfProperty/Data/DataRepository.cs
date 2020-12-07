using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using RentalOfProperty.Models;

namespace RentalOfProperty.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:MySQLConnection"];
        }

        public IEnumerable<Flat> GetFlats()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var flats = connection.Query<Flat>(@"SELECT id, address_id as addressId, type_of_house as typeOfHouse, " +
                    "number_of_rooms as numberOfRooms, price_for_month as priceForMonth, total_area as totalArea, " +
                    "additional_information as additionalInformation FROM flat;");
                foreach (var flat in flats)
                {
                    flat.Address = GetAddress(flat.AddressId);
                }
                return flats;
            }
        }

        public Flat GetFlat(int flat_id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var flat = connection.QueryFirstOrDefault<Flat>(@"SELECT id, address_id as addressId, type_of_house as typeOfHouse, " +
                    "number_of_rooms as numberOfRooms, price_for_month as priceForMonth, total_area as totalArea, " +
                    "additional_information as additionalInformation FROM flat WHERE id=@FlatId", new { FlatId = flat_id });
                flat.Address = GetAddress(flat.AddressId);
                return flat;
            }
        }

        public Address GetAddress(int address_id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var address = connection.QueryFirstOrDefault<Address>(@"SELECT id, city_id as cityId, street_id as streetId, " +
                    "house_number as houseNumber, flat_number as flatNumber FROM address WHERE id=@AddressId;",
                    new { AddressId = address_id });
                address.City = GetCity(address.CityId);
                address.Street = GetStreet(address.StreetId);
                return address;
            }
        }

        public Street GetStreet(int street_id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<Street>(@"SELECT id, name FROM street WHERE id=@StreetId;",
                    new { StreetId = street_id });
            }
        }

        public City GetCity(int city_id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<City>(@"SELECT id, name FROM city WHERE id=@CityId;",
                    new { CityId = city_id });
            }
        }
    }
}
