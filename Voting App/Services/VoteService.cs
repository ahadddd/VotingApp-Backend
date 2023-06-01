using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Voting_App.Data;
using Voting_App.Models;

namespace Voting_App.Services
{
    public class VoteService
    {
        private readonly IMongoCollection<Vote> _voteCollection;

        public VoteService(IOptions<DbSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(options.Value.DatabaseName);

            _voteCollection = mongoDb.GetCollection<Vote>("Votes");
        }

        public async Task<ICollection<Vote>> GetVotes()
        {
            return await _voteCollection.Find(vote => true).ToListAsync();
        }

        public async Task CreateVote(Vote vote)
        {
            await _voteCollection.InsertOneAsync(vote);
        }

    }
}
