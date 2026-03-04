using CriteriumBackend.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CriteriumBackend.Services
{
    public class AssignmentsService
    {
        private readonly IMongoCollection<Assignment> _assignmentsCollection;

        public AssignmentsService(IConfiguration config)
        {
            // Aquí lee la cadena que acabas de configurar en appsettings.json
            var mongoClient = new MongoClient(config.GetValue<string>("CriteriumDatabase:ConnectionString"));
            var mongoDatabase = mongoClient.GetDatabase(config.GetValue<string>("CriteriumDatabase:DatabaseName"));

            // Conecta con la colección "assignments"
            _assignmentsCollection = mongoDatabase.GetCollection<Assignment>(
                config.GetValue<string>("CriteriumDatabase:AssignmentsCollectionName"));
        }

        // 1. OBTENER TODAS (GET)
        public async Task<List<Assignment>> GetAsync() =>
            await _assignmentsCollection.Find(_ => true).ToListAsync();

        // 2. OBTENER UNA POR ID (GET ID)
        public async Task<Assignment?> GetAsync(string id) =>
            await _assignmentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // 3. CREAR NUEVA (POST)
        public async Task CreateAsync(Assignment newAssignment) =>
            await _assignmentsCollection.InsertOneAsync(newAssignment);

        // 4. ACTUALIZAR (PUT)
        public async Task UpdateAsync(string id, Assignment updatedAssignment) =>
            await _assignmentsCollection.ReplaceOneAsync(x => x.Id == id, updatedAssignment);

        // 5. ELIMINAR (DELETE)
        public async Task RemoveAsync(string id) =>
            await _assignmentsCollection.DeleteOneAsync(x => x.Id == id);

        // ==========================================================
        // 🚀 CÓDIGO SEMANA 3: OPTIMIZACIÓN Y BUSCADOR AVANZADO
        // ==========================================================
        public async Task<object> SearchDashboardAsync(string? searchQuery, int page = 1, int pageSize = 10)
        {
            var builder = Builders<Assignment>.Filter;
            var filter = builder.Empty; // Por defecto trae todo

            // Sistema de Búsqueda
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                // Regex "i" significa que ignora mayúsculas y minúsculas
                var searchRegex = new BsonRegularExpression(searchQuery, "i");
                
                // Busca si la palabra coincide con el Título o la Descripción del proyecto
                filter &= builder.Or(
                    builder.Regex(x => x.Title, searchRegex),
                    builder.Regex(x => x.Description, searchRegex)
                );
            }

            // Optimización: Paginación para no saturar la RAM (Requisito Semana 3)
            var skip = (page - 1) * pageSize;

            var resultados = await _assignmentsCollection.Find(filter)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return new 
            { 
                paginaActual = page, 
                totalResultadosEnPagina = resultados.Count,
                proyectos = resultados 
            };
        }
    }
}