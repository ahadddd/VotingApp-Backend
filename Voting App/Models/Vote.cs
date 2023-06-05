using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Voting_App.Models
{
    public class Vote
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("casted")]
        public bool Casted { get; set; } = false;

        [BsonElement("senator")]
        public string? Senator { get; set; } = string.Empty;

        [BsonElement("congressman")] 
        public string? Congressman { get; set; } = string.Empty;

        [BsonElement("votedBy")]
        public string? Voter { get; set; }
    }
}
