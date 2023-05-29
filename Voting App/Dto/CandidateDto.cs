using MongoDB.Bson.Serialization.Attributes;
using Voting_App.Models;

namespace Voting_App.Dto
{
    public class CandidateDto
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("position")]
        public string? Position { get; set; }

        //[BsonElement("votes")]
        //public ICollection<Vote>? Votes { get; set; }

        //[BsonElement("city")]
        //public City? City { get; set; }
    }
}
