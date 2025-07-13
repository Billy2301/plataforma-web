using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class InformeTratamientoViewModel
    {
        public int Hc { get; set; }

        [MaxLength(12, ErrorMessage = "El campo 'Número de Documento' debe tener 12 caracteres")]
        public string? NroDocumento { get; set; }

        [Required(ErrorMessage = "El campo 'Nombres' es obligatorio.")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo 'Sede' es obligatorio.")]
        [Display(Name = "Sede")]
        public string SedeId { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "Debe seleccionar al menos 1 opción.")]
        public List<int> OpcionesSeleccionadas { get; set; } = new List<int>();
    }


    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minElements;

        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as List<int>;

            if (list == null || list.Count < _minElements)
            {
                return new ValidationResult($"Debe seleccionar al menos {_minElements} opción(es).");
            }

            return ValidationResult.Success;
        }
    }
}
