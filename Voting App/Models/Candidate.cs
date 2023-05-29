using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Voting_App.Models
{
    public class Candidate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("position")]
        public string? Position { get; set; }

        [BsonElement("votes")]
        public ICollection<Vote>? Votes { get; set; }

        [BsonElement("city")]
        public City? City { get; set; }
        
    }
}
