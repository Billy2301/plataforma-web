using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class UsuarioViewModel
    {

        public string? Id { get; set; }
        [Display(Name = "Nombres")]
        public string? Nombre { get; set; }
        [Display(Name = "ApellidoPaterno")]
        public string? ApellidoPaterno { get; set; }
        [Display(Name = "ApellidoMaterno")]
        public string? ApellidoMaterno { get; set; }
        [Display(Name = "Activar bloqueo por intentos fallidos")]
        public bool Bloqueo { get; set; } = false;
        [Display(Name = "Correo")]
        public string? Email { get; set; }
        [Display(Name = "Perfil")]
        public int? IdPerfil { get; set; }
        [Display(Name = "Autorizar Acceso")]
        public bool EsActivo { get; set; } = true;
        [Display(Name = "Confirmar Correo")]
        public bool EsConfirmado { get; set; } = true;
        [Display(Name = "Fecha de Registro")]
        public string? fechaRegistro { get; set; }

        public string? UrlImagen { get; set; }
        // nuevas propiedades para usar roles y asignacion de un rol a un usuario
        [NotMapped]
        [Display(Name = "Rol para usuario")]
        public string IdRol { get; set; }
        [NotMapped]
        public string Rol { get; set; } = string.Empty;
        [NotMapped]
        public IEnumerable<SelectListItem> ListaRoles { get; set; } = new List<SelectListItem>();
    }
}
