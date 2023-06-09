﻿using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Voting_App.Dto;

namespace Voting_App.Models
{
    public class Candidate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("position")]
        public string? Position { get; set; }

        [BsonElement("votes")]
        public ICollection<VoteDto>? Votes { get; set; } = new List<VoteDto>();

        [BsonElement("city")]
        public string? City { get; set; } = string.Empty;
        
    }
}
