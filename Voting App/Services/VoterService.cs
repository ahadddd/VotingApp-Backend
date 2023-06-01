using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.ComponentModel;
using Voting_App.Data;
using Voting_App.Models;

namespace Voting_App.Services
{
    public class VoterService
    {
        private readonly IMongoCollection<Voter> _voterCollection;
        public VoterService(IOptions<DbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _voterCollection = mongoDb.GetCollection<Voter>("Voters");
        }

        public async Task<List<Voter>> GetVoters()
        {
            return await _voterCollection.Find(voter => true).ToListAsync();
        }

        public async Task<Voter> GetVoter(string name)
        {
            return await _voterCollection.Find(voter => voter.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Voter> GetVoterByID(string ID)
        {
            return await _voterCollection.Find(v => v.Id == ID).FirstOrDefaultAsync();
        }

        public async Task CreateVoter(Voter voter)
        {
            await _voterCollection.InsertOneAsync(voter);
        }

        public async Task UpdateVoter(Voter voter)
        {
            await _voterCollection.ReplaceOneAsync(v => v.Name == voter.Name, voter);
        }

        public async Task DeleteVoter(string name)
        {
            await _voterCollection.DeleteOneAsync(v => v.Name == name);
        }
    }
}
