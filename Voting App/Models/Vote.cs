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

        [BsonElement("votedFor")]
        public Candidate? Candidate { get; set;}

        [BsonElement("votedBy")]
        public Voter? Voter { get; set; }
    }
}
