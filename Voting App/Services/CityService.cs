using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Voting_App.Data;
using Voting_App.Models;

namespace Voting_App.Services
{
    public class CityService
    {
        private readonly IMongoCollection<City> _cityCollection;

        public CityService(IOptions<DbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _cityCollection = mongoDb.GetCollection<City>("Cities");
        }

        public async Task<City> GetCity(string name)
        {
            return await _cityCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
;       }

        public async Task<List<City>> GetCities()
        {
            return await _cityCollection.Find(city => true).ToListAsync();
        }

        public async Task CreateCity(City createCity)
        {
            await _cityCollection.InsertOneAsync(createCity);
        }

        //public async Task UpdateCity(City createCity)
        //{
        //    await _cityCollection.ReplaceOneAsync(city => city.Name == createCity.Name, createCity);
        //}

        public async Task DeleteCity(string name)
        {
            await _cityCollection.DeleteOneAsync(c => c.Name == name);
        }


    }
}
