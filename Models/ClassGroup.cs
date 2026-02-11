using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CriteriumBackend.Models
{
    public class ClassGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Section { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string TeacherId { get; set; }
        
        public List<string> StudentIds { get; set; } = new List<string>();
    }
}