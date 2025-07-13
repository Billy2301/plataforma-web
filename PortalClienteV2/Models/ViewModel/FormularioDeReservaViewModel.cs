using Entity.ClinicaModels;
using PortalClienteV2.Utilities.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class FormularioDeReservaViewModel
    {
        public _Tab1 Tab1 { get; set; } = new _Tab1();
        public _Tab2 Tab2 { get; set; } = new _Tab2();
        public _Tab3 Tab3 { get; set; } = new _Tab3();
    }


    public class _Tab1
    {
        public int Hc { get; set; } = 0;

        public string UsuarioId { get; set; } = string.Empty;

        public int Edad { get; set; } = 0;

        public int TriageOnlineID { get; set; } = 0;

        public int PagoCitaID { get; set; } = 0;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números")]
        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio.")]
        public string NombresPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números")]
        [Required(ErrorMessage = "El campo 'Apellido paterno' es obligatorio.")]
        public string ApellidoPaternoPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números")]
        [Required(ErrorMessage = "El campo 'Apellido materno' es obligatorio.")]
        public string ApellidoMaternoPaciente { get; set; }

        [Required(ErrorMessage = "El campo 'Tipo de documento' es obligatorio.")]
        public string TipoDocumento { get; set; } = "";

        [Required(ErrorMessage = "El campo 'Número de documento' es obligatorio.")]
        public string PacienteDNI { get; set; }

        [Required(ErrorMessage = "El campo 'Procedencia' es obligatorio.")]
        public string Procedencia { get; set; }

        [Required(ErrorMessage = "El campo 'Fecha de nacimiento' es obligatorio.")]
        public string FechaNacimientoPaciente { get; set; }


        public string? Colegio { get; set; }


        public string? PacienteGrado { get; set; }


        public string? NivelEducacion { get; set; }

        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [MinLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'Teléfono de contacto' debe contener solo números.")]
        [Required(ErrorMessage = "El campo 'Teléfono de contacto' es obligatorio.")]
        public string ContactoTelCelular { get; set; }

        [Required(ErrorMessage = "El campo 'Correo de contacto' es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese el 'Correo de contacto' con el formato correcto.")]
        public string ContactoCorreo { get; set; }

        public string? Parentesco { get; set; }

        public string? ApoderadoNombre { get; set; }

        public string? ContactoTipoDoc { get; set; }

        public string? ContactoNroDoc { get; set; }

        public bool GuardarEnPerfil { get; set; } = false;

        public string Dependiente { get; set; } = "NO";

    }
    public class _Tab2
    {
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MotivoConsultaLengthErrorMessage")]
        [Required(ErrorMessage = "El campo 'Motivo de consulta' es obligatorio.")]
        public string? MotivoConsulta { get; set; }

        [Required(ErrorMessage = "El campo 'Sede' es obligatorio.")]
        [Display(Name = "Sede")]
        public string? SedeID { get; set; }

    }
    public class _Tab3
    {
        public string? EspecialidadSeleccionada { get; set; }
        public string? EvaluacionSeleccionada { get; set; }
        public string? OrientacionSeleccionada { get; set; }
        public bool SolicitaScreening { get; set; } = false;
        public List<UvEspecilidadesPublica>? Especialidades { get; set; }
        public List<EvaluacionPagoViewModel>? EvaluacionesPago { get; set; }
    }


}
