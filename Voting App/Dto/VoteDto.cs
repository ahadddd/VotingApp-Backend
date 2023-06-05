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

        [BsonElement("senator")]
        public string Senator { get; set; } = string.Empty;

        [BsonElement("congressman")]
        public string Congressman { get; set; } = string.Empty;

    }
}
