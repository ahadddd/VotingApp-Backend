using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Voting_App.Models;

namespace Voting_App.Dto
{
    public class VoterDto
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string? Id { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("City")]
        public string? City { get; set; }

    }
}
