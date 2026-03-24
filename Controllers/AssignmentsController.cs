using CriteriumBackend.Models;
using CriteriumBackend.Services;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet; 
using CloudinaryDotNet.Actions; 

namespace CriteriumBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssignmentsService _assignmentsService;
        private readonly Cloudinary _cloudinary; 

        public AssignmentsController(AssignmentsService assignmentsService, Cloudinary cloudinary)
        {
            _assignmentsService = assignmentsService;
            _cloudinary = cloudinary; 
        }

        [HttpGet]
        public async Task<List<Assignment>> Get() =>
            await _assignmentsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Assignment>> Get(string id)
        {
            var assignment = await _assignmentsService.GetAsync(id);
            if (assignment is null) return NotFound();
            return assignment;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Assignment newAssignment)
        {
            await _assignmentsService.CreateAsync(newAssignment);
            return CreatedAtAction(nameof(Get), new { id = newAssignment.Id }, newAssignment);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Assignment updatedAssignment)
        {
            var assignment = await _assignmentsService.GetAsync(id);
            if (assignment is null) return NotFound();
            updatedAssignment.Id = assignment.Id;
            await _assignmentsService.UpdateAsync(id, updatedAssignment);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var assignment = await _assignmentsService.GetAsync(id);
            if (assignment is null) return NotFound();
            await _assignmentsService.RemoveAsync(id);
            return NoContent();
        }

        public class FileUrlDto
        {
            public string fileUrl { get; set; } = string.Empty;
        }

        [HttpPut("{id:length(24)}/files")]
        public async Task<IActionResult> AddFile(string id, [FromBody] FileUrlDto request)
        {
            if (string.IsNullOrWhiteSpace(request?.fileUrl))
                return BadRequest(new { mensaje = "La URL del archivo es requerida." });

            try
            {
                // This GET will safely ignore extra elements using the previously added [BsonIgnoreExtraElements]
                var assignment = await _assignmentsService.GetAsync(id);
                if (assignment is null) return NotFound();

                await _assignmentsService.AddFileUrlAsync(id, request.fileUrl);
                return Ok(new { mensaje = "Archivo agregado exitosamente al proyecto." });
            }
            catch (FormatException fex)
            {
                return StatusCode(500, new { mensaje = "Error de formato de documento (probablemente modelos viejos)", detalle = fex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchProjects([FromQuery] SearchFilterDto filters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var resultados = await _assignmentsService.SearchDashboardAsync(
                    filters.SearchQuery, filters.Page, filters.PageSize);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al buscar", detalle = ex.Message });
            }
        }

        // ==========================================================
        // 🚀 SUBIDA A LA NUBE (CLOUDINARY) 
        // ==========================================================
        [HttpPost("upload")]
        [RequestSizeLimit(104857600)] // 100MB
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { mensaje = "No se envió ningún archivo." });

            try
            {
                using var stream = file.OpenReadStream();
                string contentType = file.ContentType.ToLower();
                string fileUrl = "";

                if (contentType.StartsWith("video/"))
                {
                    var uploadParams = new VideoUploadParams() { File = new FileDescription(file.FileName, stream), Folder = "criterium_projects" };
                    var result = await _cloudinary.UploadLargeAsync(uploadParams); // 👈 Vital para videos pesados (>20MB)
                    if (result.Error != null) throw new Exception("Cloudinary Video Error: " + result.Error.Message);
                    fileUrl = result.SecureUrl.ToString();
                }
                else if (contentType.StartsWith("image/"))
                {
                    var uploadParams = new ImageUploadParams() { File = new FileDescription(file.FileName, stream), Folder = "criterium_projects" };
                    var result = await _cloudinary.UploadAsync(uploadParams);
                    if (result.Error != null) throw new Exception("Cloudinary Image Error: " + result.Error.Message);
                    fileUrl = result.SecureUrl.ToString();
                }
                else 
                {
                    var uploadParams = new RawUploadParams() { File = new FileDescription(file.FileName, stream), Folder = "criterium_projects" };
                    var result = await _cloudinary.UploadAsync(uploadParams);
                    if (result.Error != null) throw new Exception("Cloudinary Raw Error: " + result.Error.Message);
                    fileUrl = result.SecureUrl.ToString();
                }

                return Ok(new 
                { 
                    mensaje = "Archivo subido con éxito a la nube",
                    url = fileUrl, 
                    nombreOriginal = file.FileName 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al procesar el archivo.", detalle = ex.Message });
            }
        }
    }
}