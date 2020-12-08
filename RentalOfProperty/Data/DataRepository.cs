using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Dapper;
using MySql.Data.MySqlClient;
using RentalOfProperty.Models;
using RentalOfProperty.ViewModels;

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
                var flats = connection.Query<Flat>(@"SELECT id, address_id as addressId, header, type_of_house as " +
                    "typeOfHouse, number_of_rooms as numberOfRooms, price_for_month as priceForMonth, total_area as totalArea, " +
                    "additional_information as additionalInformation, balcony, owner_id as ownerId, " +
                    "flat_picture as flatPicture FROM flat;");
                foreach (var flat in flats)
                {
                    flat.Address = GetAddress(flat.AddressId);
                    flat.Owner = GetUserById(flat.OwnerId);
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
                    "additional_information as additionalInformation, balcony, header, owner_id as ownerId, flat_picture as " +
                    "flatPicture FROM flat WHERE id=@FlatId", new { FlatId = flat_id });
                flat.Address = GetAddress(flat.AddressId);
                flat.Owner = GetUserById(flat.OwnerId);
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

        public User GetUserById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(@"SELECT id, first_name as firstName, last_name as " +
                    "lastName, email, phone_number as phoneNumber FROM user WHERE id=@Id",
                    new { Id = id });
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(@"SELECT id, first_name as firstName, last_name as " +
                    "lastName, email, phone_number as phoneNumber FROM user WHERE email=@Email",
                    new { Email = email });
            }
        }

        public void InsertFlat(FlatViewModel model)
        {
            InsertAddress(model);
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO flat(flat_picture,type_of_house, number_of_rooms, price_for_month, total_area,
                                    additional_information, balcony, header, owner_id, address_id) SELECT @FlatPicture, @TypeOfHouse, @NumberOfRooms, 
                                    @PriceForMonth, @TotalArea, @AdditionalInformation, @Balcony, @Header, @OwnerId, id FROM address 
                                    WHERE city_id in (SELECT id FROM city WHERE name = @City) AND street_id in (SELECT id FROM street WHERE
                                    name = @Street) AND address.house_number = @HouseNumber AND address.flat_number = @FlatNumber;",
                new
                {
                    FlatPicture = model.FlatPicturePath,
                    model.TypeOfHouse,
                    model.NumberOfRooms,
                    model.PriceForMonth,
                    model.TotalArea,
                    AdditionalInformation = model.AdditionalInformation.Trim(),
                    model.Balcony,
                    Header = model.Header.Trim(),
                    model.OwnerId,
                    City = model.City.Trim(),
                    Street = model.Street.Trim(),
                    HouseNumber = model.HouseNumber.Trim(),
                    FlatNumber = model.FlatNumber.Trim()
                });
            }
        }

        public void InsertAddress(FlatViewModel model)
        {
            InsertCity(model.City.Trim());
            InsertStreet(model.Street.Trim());
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT IGNORE INTO Address(city_id, street_id, house_number, flat_number)
                SELECT (SELECT id from city WHERE name = @City), (SELECT id from street WHERE name = @Street), 
                @HouseNumber, @FlatNumber;",
                new { model.City, model.Street, model.HouseNumber, model.FlatNumber });
            }
        }

        public void InsertCity(string name)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT IGNORE INTO city(name) VALUES(@Name);",
                new { Name = name });
            }
        }

        public void InsertStreet(string name)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT IGNORE INTO street(name) VALUES(@Name);",
                new { Name = name });
            }
        }

        public User LoginUser(string email, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(@"SELECT id, first_name as firstName, last_name as " +
                    "lastName, email, phone_number as phoneNumber FROM user WHERE email=@Email and password=" +
                    "MD5(@Password);", new { Email = email, Password = password });
            }
        }

        public void RegisterUser(RegisterModel model)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"INSERT INTO user(first_name, last_name, email, 
                phone_number, password) VALUES (@FirstName, @LastName, @Email, @PhoneNumber, MD5(@Password));",
                new { model.FirstName, model.LastName, model.Email, model.PhoneNumber, model.Password });
            }
        }
    }
}
