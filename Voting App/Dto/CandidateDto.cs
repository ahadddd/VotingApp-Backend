using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Voting_App.Models;

namespace Voting_App.Dto
{
    public class CandidateDto
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("position")]
        public string? Position { get; set; }

        //[BsonElement("votes")]
        //public ICollection<Vote>? Votes { get; set; }

        [BsonElement("city")]
        public CityDto? City { get; set; }
    }
}
