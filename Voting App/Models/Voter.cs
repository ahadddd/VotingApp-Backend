using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Voting_App.Dto;

namespace Voting_App.Models
{
    public class Voter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("City")]
        public City? City { get; set; }

        [BsonElement("Vote")]
        public VoteDto? VoteCasted { get; set; }
    }
}
