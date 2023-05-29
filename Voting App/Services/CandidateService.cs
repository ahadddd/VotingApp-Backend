﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Voting_App.Data;
using Voting_App.Models;

namespace Voting_App.Services
{
    public class CandidateService
    {
        private readonly IMongoCollection<Candidate> _candidateCollection;
        public CandidateService(IOptions<DbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDB = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _candidateCollection = mongoDB.GetCollection<Candidate>("Candidates");
        }

        public async Task<List<Candidate>> GetCandidates()
        {
            return await _candidateCollection.Find(candidate => true).ToListAsync();
        }

        public async Task<Candidate> GetCandidate(string name)    
        {
            return await _candidateCollection.Find(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }

    }
}
