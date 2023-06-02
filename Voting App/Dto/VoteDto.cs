using MongoDB.Bson.Serialization.Attributes;
using Voting_App.Models;

namespace Voting_App.Dto
{
    public class VoteDto
    {
        [BsonElement("casted")]
        public bool Casted { get; set; }

        [BsonElement("votedBy")]
        public string? Voter { get; set; }

        [BsonElement("votedFor")]
        public string? Candidate { get; set; }

    }
}
