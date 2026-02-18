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
    }
}