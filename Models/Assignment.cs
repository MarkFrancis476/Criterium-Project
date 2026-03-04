using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CriteriumBackend.Models
{
    public class Assignment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ClassGroupId { get; set; }

        public string? AttachedFileUrl { get; set; }
        public string? AttachedFileName { get; set; }

        public List<string> EvaluatorComments { get; set; } = new List<string>();

        public List<RubricCriteria> Rubric { get; set; } = new List<RubricCriteria>();
    }

    public class RubricCriteria
    {
        public string CriteriaName { get; set; } = string.Empty;
        public int MaxPoints { get; set; }
    }
}