using PortalClienteV2.Utilities.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class SolicitudViewModel
    {
        public Tab_1 Tab1 { get; set; } = new Tab_1();
        public Tab_2 Tab2 { get; set; } = new Tab_2();
        public Tab_3 Tab3 { get; set; } = new Tab_3();

        public int TriageOnlineID { get; set; } = 0;

    }

    public class Tab_1
    {
        [Required(ErrorMessage = "Error al cargar el triaje: 00XX01151")]
        public int Hc { get; set; } = 0;

        [Required(ErrorMessage = "Error al cargar el triaje: 00XX01152")]
        public string UsuarioId { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números ni espacios en blanco")]
        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio.")]
        [Display(Name = "Nombres")]
        public string NombresPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El apellido paterno no debe contener números ni espacios en blanco")]
        [Required(ErrorMessage = "El campo 'Apellido paterno' es obligatorio.")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaternoPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El apellido materno no debe contener números ni espacios en blanco")]
        [Required(ErrorMessage = "El campo 'Apellido materno' es obligatorio.")]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaternoPaciente { get; set; }

        [Required(ErrorMessage = "El campo 'Tipo de documento' es obligatorio.")]
        [Display(Name = "Tipo de Documento")]
        public string TipoDocumento { get; set; } = "";

        //[RegularExpression(@"^\d+$", ErrorMessage = "El Nro. DNI debe contener solo números.")]
        [Required(ErrorMessage = "El campo 'Número. de documento' es obligatorio.")]
        [Display(Name = "El Nro. documento")]
        public string PacienteDNI { get; set; }

        [Required(ErrorMessage = "El campo 'Procedencia' es obligatorio.")]
        [Display(Name = "Procedencia")]
        public string Procedencia { get; set; } = "";

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "El formato de fecha debe ser dd/mm/aaaa.")]
        [Required(ErrorMessage = "El campo 'Fecha de nacimiento' es obligatorio.")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public string FechaNacimientoPaciente { get; set; }

        //[Required(ErrorMessage = "El campo 'Colegio' es obligatorio.")]
        [Display(Name = "Colegio")]
        public string? Colegio { get; set; }

        //[Required(ErrorMessage = "El campo 'Grado' es obligatorio.")]
        [Display(Name = "Grado")]
        public string? PacienteGrado { get; set; }

        [Display(Name = "Nivel de Educacion")]
        public string NivelEducacion { get; set; } = "";

        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [MinLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'Teléfono de contacto' debe contener solo números.")]
        [Required(ErrorMessage = "El campo 'Teléfono de contacto' es obligatorio.")]
        [Display(Name = "Teléfono de contacto")]
        public string ApoderadoTelCelular { get; set; } = "";

        [Required(ErrorMessage = "El campo 'Correo de contacto' es obligatorio.")]
        [Display(Name = "Correo de contacto")]
        [EmailAddress(ErrorMessage = "Ingrese el 'Correo de contacto' con el formato correcto.")]
        public string EmailRegistro { get; set; } = "";


        [Display(Name = "Nivel de parentesco")]
        public string? ApoderadoRelacion { get; set; } = "";

        [RegularExpression(pattern: @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El campo 'Nombres' no debe contener números ni espacios en blanco.")]
        [Display(Name = "Nombres")]
        public string? ApoderadoNombre { get; set; } = "";

        [Display(Name = "Tipo de documento")]
        public string? ContactoTipoDoc { get; set; } = "";

        //[RegularExpression(@"^\d+$", ErrorMessage = "El Nro de Documento de Contacto debe contener solo números.")]
        [Display(Name = "Número de documento")]
        public string? ContactoNroDoc { get; set; } = "";

        public bool GuardarEnPerfil { get; set; } = false;

        [Display(Name = "Dependiente")]
        public string Dependiente { get; set; } = "SI";

    }

    public class Tab_2
    {
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MotivoConsultaLengthErrorMessage")]
        [Required(ErrorMessage = "El campo 'Motivo de consulta' es obligatorio.")]
        [Display(Name = "Motivo de consulta")]
        public string MotivoConsulta { get; set; }

        [Required(ErrorMessage = "El campo 'Sede' es obligatorio.")]
        [Display(Name = "Sede")]
        public int SedeID { get; set; } = 0;

    }

    public class Tab_3
    {
        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Neurología")]
        public bool EvaluarNeurologia { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Psicología")]
        public bool EvaluarPsicologia { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Lenguaje y Aprendizaje")]
        public bool EvaluarLenguaje { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Psicomotriz")]
        public bool EvaluarCuerpoMovimiento { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Audiología")]
        public bool EvaluarAudiologia { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Habla")]
        public bool EvaluarHabla { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evaluar Aprendizaje")]
        public bool EvaluaAprendizaje { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evalua Udad")]
        public bool EvaluarUdad { get; set; } = false;

        [AtLeastOneTrue(ErrorMessage = "Al menos debe seleccionar una especialidad")]
        [Display(Name = "Evalua Orientacion")]
        public bool EvaluarOrientacion { get; set; } = false;
    }
}
