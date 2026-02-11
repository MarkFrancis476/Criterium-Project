using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CriteriumBackend.Models
{
    public class Assignment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClassGroupId { get; set; }

        // Aquí está la parte "Embebida" (Rúbrica dentro de la Tarea)
        public List<RubricCriteria> Rubric { get; set; } = new List<RubricCriteria>();
    }

    public class RubricCriteria
    {
        public string CriteriaName { get; set; }
        public int MaxPoints { get; set; }
    }
}