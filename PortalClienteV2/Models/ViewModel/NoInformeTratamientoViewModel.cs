using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class NoInformeTratamientoViewModel
    {
        public string OrientacionSeleccionada { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'Teléfono de contacto' debe contener solo números.")]
        [MinLength(9, ErrorMessage = "El campo 'Teléfono de contacto' debe tener 9 caracteres")]
        [MaxLength(9, ErrorMessage = "El campo 'Teléfono de contacto' debe tener 9 caracteres")]
        public string? ContactoTelefono { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese el 'Correo de contacto' con el formato correcto.")]
        public string? ContactoCorreo { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'Número de WhatsApp' debe contener solo números.")]
        [MinLength(9, ErrorMessage = "El campo 'Número de WhatsApp' debe tener 9 caracteres")]
        [MaxLength(9, ErrorMessage = "El campo 'Número de WhatsApp' debe tener 9 caracteres")]
        public string? ContactoWhatsApp { get; set; }
        public bool flag { get; set; } = true;
    }
}
