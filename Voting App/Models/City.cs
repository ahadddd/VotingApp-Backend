using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Voting_App.Models
{
    public class City
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Candidates")]
        public ICollection<Candidate>? Candidates { get; set; }


    }
}
