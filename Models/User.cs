using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CriteriumBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Ejemplo: "Teacher", "Student"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}