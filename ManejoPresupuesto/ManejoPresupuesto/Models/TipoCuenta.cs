

using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo del {0} es requerido")] //==> {0} el uso del placeholder sustituya el nombre del campo
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo del {0} debe tener un minimo de {2} y un maximo de {1}")]
        [Display(Name = "Nombre de tipo de cuenta")] //==> Modifica el nombre del formulario con el que esta sincronizado el campo
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Order { get; set; }

        /*  Puebas de otras validaciones por defecto */

        [Required(ErrorMessage = "El campo del {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo del {0} debe ser un email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Range(minimum: 18, maximum: 100, ErrorMessage = "El campo del {0} debe estar entre {1} y {2}")]
        public int Edad { get; set; }

        [Url(ErrorMessage = "El campo del {0} debe ser una url")]
        [Display(Name = "UrlXXX")]
        public string Url { get; set; }

        [CreditCard(ErrorMessage = "La tc no es valida")]
        [Display(Name = "Tarjeta de Credito")]
        public string TarjetaDeCredito { get; set; }
    }
}