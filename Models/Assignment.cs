using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CriteriumBackend.Models
{
    [BsonIgnoreExtraElements] // Protege contra campos viejos
    public class Assignment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("subject")]
        public string Subject { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("members")]
        public List<string> Members { get; set; } = new List<string>();

        [BsonElement("technologies")]
        public List<string> Technologies { get; set; } = new List<string>();

        [BsonElement("status")]
        public string Status { get; set; } = "Pendiente";

        // 🔥 LA SOLUCIÓN AL CRASH 🔥
        [BsonElement("fileUrls")]
        public List<string> FileUrls { get; set; } = new List<string>();

        [BsonElement("coverImageUrl")]
        public string CoverImageUrl { get; set; } = string.Empty;

        [BsonElement("time")]
        public string Time { get; set; } = string.Empty;

        // Mantenemos estos para que no exploten los proyectos muy viejos
        [BsonElement("fileUrl")]
        public string? FileUrl { get; set; }
        
        [BsonElement("attachedFileUrl")]
        public string? AttachedFileUrl { get; set; }
    }
}