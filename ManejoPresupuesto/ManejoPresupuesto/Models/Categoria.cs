

using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Nopuede ser mayor a {1} caracteres")]
        public string Nombre { get; set; }
        [Display(Name = "Tipo de Operación")]
        public TipoOperacion TipoOperacionId { get; set; }
        public int UsuarioId { get; set; }
    }
}