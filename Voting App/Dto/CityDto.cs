using MongoDB.Bson.Serialization.Attributes;

namespace Voting_App.Dto
{
    public class CityDto
    {
        [BsonElement("Name")]
        public string? Name { get; set; }
    }
}
