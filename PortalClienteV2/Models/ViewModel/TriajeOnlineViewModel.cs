using PortalClienteV2.Utilities.Helpers;
using System.ComponentModel.DataAnnotations;


namespace PortalClienteV2.Models.ViewModel
{
    public class TriajeOnlineViewModel
    {
        public Tab1 Tab1 { get; set; } = new Tab1();
        public Tab2 Tab2 { get; set; } = new Tab2();
        public Tab3 Tab3 { get; set; } = new Tab3();
        public Tab4 Tab4 { get; set; } = new Tab4();
        public Tab5 Tab5 { get; set; } = new Tab5();

        public int TriageOnlineID { get; set; } = 0;

    }

    public class Tab1
    {
        [Required(ErrorMessage = "Error al cargar el triaje: 00XX01151")]
        public int Hc { get; set; } = 0;

        [Required(ErrorMessage = "Error al cargar el triaje: 00XX01152")]
        public string UsuarioId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo 'Sexo de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Sexo de la persona en atenderse")]
        public byte? SexoPaciente { get; set; } = 0;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números ni espacios en blanco")]
        [Required(ErrorMessage = "El campo 'Nombres de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Nombres de la persona en atenderse")]
        public string NombresPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El apellido paterno no debe contener números ni espacios en blanco")]
        [Required(ErrorMessage = "El campo 'Apellido Paterno de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Apellido Paterno de la persona en atenderse")]
        public string ApellidoPaternoPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El apellido materno no debe contener números ni espacios en blanco")]
        [Required(ErrorMessage = "El campo 'Apellido Materno de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Apellido Materno de la persona en atenderse")]
        public string ApellidoMaternoPaciente { get; set; }

        [Required(ErrorMessage = "El campo 'Tipo de Documento de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Tipo de Documento")]
        public string TipoDocumento { get; set; } = "";

        //[RegularExpression(@"^\d+$", ErrorMessage = "El Nro. DNI debe contener solo números.")]
        [Required(ErrorMessage = "El campo 'Nro. de documento de la persona en atenderse' es obligatorio.")]
        [Display(Name = "El Nro. documento de la persona en atenderse")]
        public string PacienteDNI { get; set; }

        [Required(ErrorMessage = "El campo 'Procedencia de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Procedencia")]
        public string Procedencia { get; set; } = "";

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "El formato de fecha debe ser dd/mm/aaaa.")]
        [Required(ErrorMessage = "El campo 'Fecha de Nacimiento de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Fecha de Nacimiento de la persona en atenderse")]
        [DataType(DataType.Date)]
        public string FechaNacimientoPaciente { get; set; }

        //[Required(ErrorMessage = "El campo 'Colegio de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Colegio")]
        public string? Colegio { get; set; }

        //[Required(ErrorMessage = "El campo 'Grado de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Grado")]
        public string? PacienteGrado { get; set; }

        [Required(ErrorMessage = "El campo 'Nivel de Educacion de la persona en atenderse' es obligatorio.")]
        [Display(Name = "Nivel de Educacion")]
        public string NivelEducacion { get; set; } = "";

    }
    public class Tab2
    {
        [Display(Name = "Dependiente")]
        public string Dependiente { get; set; } = "SI";

        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [MinLength(9, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "TelefonoLengthErrorMessage")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo 'Teléfono Celular' debe contener solo números.")]
        [Required(ErrorMessage = "El campo 'Teléfono Celular del Apoderado' es obligatorio.")]
        [Display(Name = "Teléfono Celular del Apoderado")]
        public string ApoderadoTelCelular { get; set; } = "";

        [Required(ErrorMessage = "El campo 'Email de Registro' es obligatorio.")]
        [Display(Name = "Email de Registro")]
        [EmailAddress(ErrorMessage = "Ingrese el 'Email de Registro' con el formato correcto.")]
        public string EmailRegistro { get; set; } = "";

        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Sexo de Contacto' es obligatorio.")]
        [Display(Name = "Sexo de Contacto")]
        public byte? ContactoSexo { get; set; } = 0;

        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Relación con el Apoderado' es obligatorio.")]
        [Display(Name = "Relación con el Apoderado")]
        public string? ApoderadoRelacion { get; set; } = "";

        [RegularExpression(pattern: @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre del apoderado no debe contener números ni espacios en blanco.")]
        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Nombre del Apoderado' es obligatorio.")]
        [Display(Name = "Nombre del Apoderado")]
        public string? ApoderadoNombre { get; set; } = "";

        [RegularExpression(pattern: @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El Ape. paterno del Apoderado no debe contener números ni espacios en blanco.")]
        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Ape. paterno del Apoderado' es obligatorio.")]
        [Display(Name = "Ape. paterno del Apoderado")]
        public string? ApoderadoApellidoPaterno { get; set; } = "";

        [RegularExpression(pattern: @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El Ape. materno del Apoderado no debe contener números ni espacios en blanco.")]
        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Ape. materno del Apoderado' es obligatorio.")]
        [Display(Name = "Ape. materno del Apoderado")]
        public string? ApoderadoApellidoMaterno { get; set; } = "";

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "El formato de fecha debe ser dd/mm/aaaa.")]
        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Fecha de Nacimiento de Contacto' es obligatorio.")]
        [Display(Name = "Fecha de Nacimiento de Contacto")]
        [DataType(DataType.Date)]
        public string? ContactoFechaNac { get; set; } = "";

        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Tipo de Documento de Contacto' es obligatorio.")]
        [Display(Name = "Tipo de Documento de Contacto")]
        public string? ContactoTipoDoc { get; set; } = "";

        //[RegularExpression(@"^\d+$", ErrorMessage = "El Nro de Documento de Contacto debe contener solo números.")]
        [RequiredIfDependiente("SI", ErrorMessage = "El campo 'Nro de Documento de Contacto' es obligatorio.")]
        [Display(Name = "Nro de Documento de Contacto")]
        public string? ContactoNroDoc { get; set; } = "";

        public bool GuardarEnPerfil { get; set; } = false;

    }
    public class Tab3
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
        [Display(Name = "Evalua Otro")]
        public bool EvaluarOtro { get; set; } = false;
    }
    public class Tab4
    {
        public string? SelectAudiologia { get; set; }
        public string? SelectHabla { get; set; }
        public string? SelectUdad { get; set; }
        public string? SelectCuerpo { get; set; }
        public string? SelectPsicologia { get; set; }
        public string? SelectAprendizaje { get; set; }
        public string Lenguaje01 { get; set; } = "NS"; // "SI", "NO", "NS" = No Seleccionado
        public string Lenguaje02 { get; set; } = "NS";
        public string Lenguaje03 { get; set; } = "NS";
        public string Lenguaje04 { get; set; } = "NS";
        public string Lenguaje05 { get; set; } = "NS";
        public string Lenguaje06 { get; set; } = "NS";
        public string Lenguaje07 { get; set; } = "NS";
        public string Lenguaje08 { get; set; } = "NS";
        public string Lenguaje09 { get; set; } = "NS";
        public string Lenguaje10 { get; set; } = "NS";
        public string Lenguaje11 { get; set; } = "NS";
        public string Lenguaje12 { get; set; } = "NS";
        public string Lenguaje13 { get; set; } = "NS";
        public string Habla01 { get; set; } = "NS";
        public string Habla02 { get; set; } = "NS";
        public string Habla03 { get; set; } = "NS";
        public string Habla04 { get; set; } = "NS";
        public string Habla05 { get; set; } = "NS";
        public string Habla06 { get; set; } = "NS";
        public string Habla07 { get; set; } = "NS";
        public string Habla08 { get; set; } = "NS";
        public string Habla09 { get; set; } = "NS";
        public string Habla10 { get; set; } = "NS";
        public string Habla11 { get; set; } = "NS";
        public string Habla12 { get; set; } = "NS";
        public string Habla13 { get; set; } = "NS";
        public string Habla14 { get; set; } = "NS";
        public string Habla15 { get; set; } = "NS";
        public string Habla16 { get; set; } = "NS";
        public string Habla17 { get; set; } = "NS";
        public string Habla18 { get; set; } = "NS";
        //public string Aprend01 { get; set; } = "NS";
        //public string Aprend02 { get; set; } = "NS";
        //public string Aprend03 { get; set; } = "NS";
        //public string Aprend04 { get; set; } = "NS";
        //public string Aprend05 { get; set; } = "NS";
        //public string Aprend06 { get; set; } = "NS";
        //public string Aprend07 { get; set; } = "NS";
        //public string Aprend08 { get; set; } = "NS";
        //public string Aprend09 { get; set; } = "NS";
        //public string Aprend10 { get; set; } = "NS";
        //public string Aprend11 { get; set; } = "NS";
        //public string Aprend12 { get; set; } = "NS";
        //public string Aprend13 { get; set; } = "NS";
        public string AprendPre01 { get; set; } = "NS";
        public string AprendPre02 { get; set; } = "NS";
        public string AprendPre03 { get; set; } = "NS";
        public string AprendPre04 { get; set; } = "NS";
        public string AprendPre05 { get; set; } = "NS";
        public string AprendPre06 { get; set; } = "NS";
        public string AprendPre07 { get; set; } = "NS";
        public string AprendPre08 { get; set; } = "NS";
        public string AprendPre09 { get; set; } = "NS";
        public string AprendPre10 { get; set; } = "NS";
        public string AprendEsc01 { get; set; } = "NS";
        public string AprendEsc02 { get; set; } = "NS";
        public string AprendEsc03 { get; set; } = "NS";
        public string AprendEsc04 { get; set; } = "NS";
        public string AprendEsc05 { get; set; } = "NS";
        public string AprendEsc06 { get; set; } = "NS";
        public string AprendEsc07 { get; set; } = "NS";
        public string AprendEsc08 { get; set; } = "NS";
        public string AprendEsc09 { get; set; } = "NS";
        public string AprendEsc10 { get; set; } = "NS";
        public string AprendEsc11 { get; set; } = "NS";
        public string AprendEsc12 { get; set; } = "NS";
        public string AprendEsc13 { get; set; } = "NS";
        public string AprendEsc14 { get; set; } = "NS";
        public string AprendEsc15 { get; set; } = "NS";
        public string AprendUni01 { get; set; } = "NS";
        public string AprendUni02 { get; set; } = "NS";
        public string AprendUni03 { get; set; } = "NS";
        public string AprendUni04 { get; set; } = "NS";
        public string AprendUni05 { get; set; } = "NS";
        public string AprendUni06 { get; set; } = "NS";
        public string AprendUni07 { get; set; } = "NS";
        public string AprendUni08 { get; set; } = "NS";
        public string AprendAdul01 { get; set; } = "NS";
        public string AprendAdul02 { get; set; } = "NS";
        public string AprendAdul03 { get; set; } = "NS";
        public string AprendAdul04 { get; set; } = "NS";
        public string AprendAdul05 { get; set; } = "NS";
        public string AprendAdul06 { get; set; } = "NS";
        public string Cuerpo01 { get; set; } = "NS";
        public string Cuerpo02 { get; set; } = "NS";
        public string Cuerpo03 { get; set; } = "NS";
        public string Cuerpo04 { get; set; } = "NS";
        public string Cuerpo05 { get; set; } = "NS";
        public string Cuerpo06 { get; set; } = "NS";
        public string Cuerpo07 { get; set; } = "NS";
        public string Cuerpo08 { get; set; } = "NS";
        public string Cuerpo09 { get; set; } = "NS";
        public string Cuerpo10 { get; set; } = "NS";
        public string Cuerpo11 { get; set; } = "NS";
        public string Cuerpo12 { get; set; } = "NS";
        public string Cuerpo13 { get; set; } = "NS";
        public string Cuerpo14 { get; set; } = "NS";
        public string UdadPL01 { get; set; } = "NS";
        public string UdadPL02 { get; set; } = "NS";
        public string UdadPL03 { get; set; } = "NS";
        public string UdadPL04 { get; set; } = "NS";
        public string UdadPL05 { get; set; } = "NS";
        public string UdadPL06 { get; set; } = "NS";
        public string UdadPL07 { get; set; } = "NS";
        public string UdadJuego01 { get; set; } = "NS";
        public string UdadJuego02 { get; set; } = "NS";
        public string UdadEstereo01 { get; set; } = "NS";
        public string UdadEstereo02 { get; set; } = "NS";
        public string UdadSensorial01 { get; set; } = "NS";
        public string UdadSensorial02 { get; set; } = "NS";
        public string UdadSensorial03 { get; set; } = "NS";
        public string UdadSensorial04 { get; set; } = "NS";
        public string UdadLF01 { get; set; } = "NS";
        public string UdadLF02 { get; set; } = "NS";
        public string UdadLF03 { get; set; } = "NS";
        public string UdadLF04 { get; set; } = "NS";
        public string UdadLF05 { get; set; } = "NS";
        public string UdadLF06 { get; set; } = "NS";
        public string UdadConduc01 { get; set; } = "NS";
        public string UdadConduc02 { get; set; } = "NS";
        public string UdadConduc03 { get; set; } = "NS";
        public string UdadConduc04 { get; set; } = "NS";
        public string UdadFSenso01 { get; set; } = "NS";
        public string UdadFSenso02 { get; set; } = "NS";
        public string AudioProcAud01 { get; set; } = "NS";
        public string AudioProcAud02 { get; set; } = "NS";
        public string AudioProcAud03 { get; set; } = "NS";
        public string AudioProcAud04 { get; set; } = "NS";
        public string AudioProcAud05 { get; set; } = "NS";
        public string AudioProcAud06 { get; set; } = "NS";
        public string AudioProcAud07 { get; set; } = "NS";
        public string AudioProcAud08 { get; set; } = "NS";
        public string AudioProcAud09 { get; set; } = "NS";
        public string AudioProcAud10 { get; set; } = "NS";
        public string AudioPerdBebe01 { get; set; } = "NS";
        public string AudioPerdBebe02 { get; set; } = "NS";
        public string AudioPerdBebe03 { get; set; } = "NS";
        public string AudioPerdBebe04 { get; set; } = "NS";
        public string AudioPerdBebe05 { get; set; } = "NS";
        public string AudioPerdBebe06 { get; set; } = "NS";
        public string AudioPerdBebe07 { get; set; } = "NS";
        public string AudioPerdBebe08 { get; set; } = "NS";
        public string AudioPerdBebe09 { get; set; } = "NS";
        public string AudioPerdBebe10 { get; set; } = "NS";
        public string AudioPerdAdul01 { get; set; } = "NS";
        public string AudioPerdAdul02 { get; set; } = "NS";
        public string AudioPerdAdul03 { get; set; } = "NS";
        public string AudioPerdAdul04 { get; set; } = "NS";
        public string AudioPerdAdul05 { get; set; } = "NS";
        public string AudioPerdAdul06 { get; set; } = "NS";
        public string AudioPerdAdul07 { get; set; } = "NS";
        public string AudioPerdAdul08 { get; set; } = "NS";
        public string AudioPerdAdul09 { get; set; } = "NS";
        public string AudioPerdAdul10 { get; set; } = "NS";

    }
    public class Tab5
    {
        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MotivoConsultaLengthErrorMessage")]
        [Required(ErrorMessage = "El campo 'Motivo de Consulta' es obligatorio.")]
        [Display(Name = "Motivo de Consulta")]
        public string MotivoConsulta { get; set; }

        [Required(ErrorMessage = "El campo 'Cómo llegó al CPAL' es obligatorio.")]
        [Display(Name = "Cómo llegó al CPAL")]
        public byte? ComoLlegoCPAL { get; set; } = 0;

        [Required(ErrorMessage = "El campo 'Consultó Por' es obligatorio.")]
        [Display(Name = "Consultó Por")]
        public string ConsultoPor { get; set; } = "";

        [Required(ErrorMessage = "El campo 'ID de Sede' es obligatorio.")]
        [Display(Name = "ID de Sede")]
        public int SedeID { get; set; } = 0;

    }



    public class RequiredIfDependiente : ValidationAttribute
    {
        private readonly string _isRequired;

        public RequiredIfDependiente(string isRequired)
        {
            _isRequired = isRequired;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (Tab2)validationContext.ObjectInstance;
            if (viewModel.Dependiente == _isRequired)
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    var fieldName = validationContext.DisplayName; // Obtiene el nombre del campo
                    var errorMessage = $"El campo '{fieldName}' es obligatorio."; // Concatena el nombre del campo con el mensaje
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    public class AtLeastOneTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (Tab3)validationContext.ObjectInstance;
            var properties = validationContext.ObjectType.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(bool) && (bool)property.GetValue(viewModel))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage ?? "Por favor, seleccione al menos una especialidad.");
        }
    }

}
