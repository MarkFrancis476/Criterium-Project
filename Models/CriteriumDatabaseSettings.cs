namespace CriteriumBackend.Models
{
    public class CriteriumDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string AssignmentsCollectionName { get; set; } = null!;
    }
}