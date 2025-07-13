using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class RecuperarContraseñaViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
