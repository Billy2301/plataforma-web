using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class RestablecerContraseñaViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmacion de contraseña no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [MinLength(6, ErrorMessage = "La confirmación de la contraseña debe tener al menos 6 caracteres")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
