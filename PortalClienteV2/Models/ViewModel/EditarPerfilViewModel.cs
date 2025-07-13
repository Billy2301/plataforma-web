using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalClienteV2.Models.ViewModel
{
    public class EditarPerfilViewModel
    {
        [Key]
        public int Id { get; set; } = 0;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El campo Nombres no debe contener números")]
        [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El campo Apellidos no debe contener números")]
        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Apellido paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El campo Apellidos no debe contener números")]
        [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Apellido materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Display(Name = "País")]
        public int? Pais { get; set; } = 0;

        [Display(Name = "Región")]
        public int? Region { get; set; } = 0;

        [Display(Name = "Departamento")]
        public int? Departamento { get; set; } = 0;

        [StringLength(300)]
        [Display(Name = "Dirección")]
        public string? Dirección { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo teléfono es obligatorio.")]
        [StringLength(12)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(50)]
        [Required(ErrorMessage = "El campo 'Tipo de documento' es obligatorio.")]
        [Display(Name = "Tipo de documento")]
        public string TipoDoc { get; set; } = string.Empty;

        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se permiten números.")]
        [StringLength(12, ErrorMessage = "Error: máximo 12 caracteres permitidos.")]
        [Required(ErrorMessage = "El campo 'DNI' es obligatorio.")]
        [Display(Name = "DNI")]
        public string DNI { get; set; } = string.Empty;

        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; } = DateTime.MinValue;

        [StringLength(100)]
        [Display(Name = "Imagen interna")]
        public string? ImagenInternal { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Imagen externa")]
        public string? ImagenExternal { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Extensión de la imagen")]
        public string? ImagenExtension { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "URL de la imagen")]
        public string? UrlImagen { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo 'Sexo' es obligatorio.")]
        [Range(1, 2, ErrorMessage = "El campo 'Sexo' es obligatorio")]
        [Display(Name = "Sexo")]
        public byte? Sexo { get; set; } = 0;

        public string? ActualizadoPor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo fecha de creación es obligatorio.")]
        [Display(Name = "Fecha de creación")]
        public DateTime? CreadoFecha { get; set; } = DateTime.MinValue;

        public string? RefAspNetUser { get; set; } = string.Empty;

        [NotMapped]
        public string? FilePath { get; set; } = string.Empty;

        [NotMapped]
        public IEnumerable<SelectListItem> listTipoDoc { get; set; } = new List<SelectListItem>(){
                    new SelectListItem { Text = "DNI", Value = "DNI" },
                    new SelectListItem { Text = "CARNET DE EXTRANJERIA", Value = "CARNET_EXT" },
                    new SelectListItem { Text = "PASAPORTE", Value = "PASAPORTE" }
                };
        [NotMapped]
        public IEnumerable<SelectListItem> listPaises { get; set; } = new List<SelectListItem>();
        [NotMapped]
        public IEnumerable<SelectListItem> listRegiones { get; set; } = new List<SelectListItem>();
        [NotMapped]
        public IEnumerable<SelectListItem> listDepartamentos { get; set; } = new List<SelectListItem>();

    }
}
