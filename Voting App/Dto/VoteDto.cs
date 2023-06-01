using MongoDB.Bson.Serialization.Attributes;
using Voting_App.Models;

namespace Voting_App.Dto
{
    public class VoteDto
    {
        [BsonElement("casted")]
        public bool Casted { get; set; }

        [BsonElement("votedBy")]
        public string Voter { get; set; } = string.Empty;

        [BsonElement("votedFor")]
        public string Candidate { get; set; } = string.Empty;

    }
}
