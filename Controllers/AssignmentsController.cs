using CriteriumBackend.Models;
using CriteriumBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CriteriumBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La ruta será: api/assignments
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentsService _assignmentsService;

        public AssignmentsController(AssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        // GET: api/assignments (Obtener todas)
        [HttpGet]
        public async Task<List<Assignment>> Get() =>
            await _assignmentsService.GetAsync();

        // GET: api/assignments/{id} (Obtener una)
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Assignment>> Get(string id)
        {
            var assignment = await _assignmentsService.GetAsync(id);

            if (assignment is null)
            {
                return NotFound();
            }

            return assignment;
        }

        // POST: api/assignments (Crear nueva)
        [HttpPost]
        public async Task<IActionResult> Post(Assignment newAssignment)
        {
            await _assignmentsService.CreateAsync(newAssignment);
            return CreatedAtAction(nameof(Get), new { id = newAssignment.Id }, newAssignment);
        }

        // PUT: api/assignments/{id} (Actualizar)
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Assignment updatedAssignment)
        {
            var assignment = await _assignmentsService.GetAsync(id);

            if (assignment is null)
            {
                return NotFound();
            }

            updatedAssignment.Id = assignment.Id;
            await _assignmentsService.UpdateAsync(id, updatedAssignment);

            return NoContent();
        }

        // DELETE: api/assignments/{id} (Borrar)
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var assignment = await _assignmentsService.GetAsync(id);

            if (assignment is null)
            {
                return NotFound();
            }

            await _assignmentsService.RemoveAsync(id);

            return NoContent();
        }

        // ==========================================================
        // 🚀 ENDPOINT SEMANA 3: BÚSQUEDA CON MANEJO DE ERRORES
        // ==========================================================
        [HttpGet("Search")]
        public async Task<IActionResult> SearchProjects([FromQuery] SearchFilterDto filters)
        {
            // 1. Validaciones robustas (Requisito Semana 3)
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                { 
                    mensaje = "Parámetros de búsqueda inválidos", 
                    errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) 
                });
            }

            try
            {
                // 2. Llamada al servicio optimizado
                var resultados = await _assignmentsService.SearchDashboardAsync(
                    filters.SearchQuery, 
                    filters.Page, 
                    filters.PageSize
                );
                
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                // 3. Manejo de errores para que el servidor no se caiga
                return StatusCode(500, new 
                { 
                    mensaje = "Error interno al buscar los proyectos en la base de datos.", 
                    detalle = ex.Message 
                });
            }
        }
    }
}