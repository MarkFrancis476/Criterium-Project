using System.ComponentModel.DataAnnotations;

namespace CriteriumBackend.Models
{
    public class SearchFilterDto
    {
        [MaxLength(100, ErrorMessage = "La búsqueda no puede exceder los 100 caracteres.")]
        public string? SearchQuery { get; set; }

        public string? StatusFilter { get; set; }

        [Range(1, 100, ErrorMessage = "El número de página debe ser mayor a 0.")]
        public int Page { get; set; } = 1;

        [Range(1, 50, ErrorMessage = "El tamaño de página debe estar entre 1 y 50.")]
        public int PageSize { get; set; } = 10;
    }
}