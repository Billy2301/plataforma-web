using System.ComponentModel.DataAnnotations;

namespace PortalClienteV2.Models.ViewModel
{
    public class ObtenTuCitaViewModel
    {
        public Tab1_ Tab1 { get; set; } = new Tab1_();
        public Tab2_ Tab2 { get; set; } = new Tab2_();
        public Tab3_ Tab3 { get; set; } = new Tab3_();
        public int TriageOnlineID { get; set; } = 0;
    }

    public class Tab1_
    {
        public int Hc { get; set; } = 0;

        public string UsuarioId { get; set; } = string.Empty;

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string NombresPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ApellidoPaternoPaciente { get; set; }

        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+( [a-zA-ZñÑáéíóúÁÉÍÓÚ]+)*\s*", ErrorMessage = "El nombre no debe contener números")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ApellidoMaternoPaciente { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string TipoDocumento { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string PacienteDNI { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string Procedencia { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string FechaNacimientoPaciente { get; set; }


        public string? Colegio { get; set; }


        public string? PacienteGrado { get; set; }


        public string? NivelEducacion { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ContactoTelCelular { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string ContactoCorreo { get; set; }

        public string? Parentesco { get; set; }

        public string? ApoderadoNombre { get; set; }

        public string? ContactoTipoDoc { get; set; }

        public string? ContactoNroDoc { get; set; }

        public bool GuardarEnPerfil { get; set; } = false;

        public string Dependiente { get; set; } = "NO";

    }
    public class Tab2_
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string? MotivoConsulta { get; set; }

        public int SedeID { get; set; } = 0;

    }
    public class Tab3_
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

        public List<EspecialidadViewModel> Especialidades { get; set; }
    }

    public class EspecialidadViewModel
    {
        public int EspecialidadID { get; set; }
        public string Especialidad { get; set; } // Nombre de la especialidad
        public string Area { get; set; }
        public List<EvaluacionViewModel> Evaluaciones { get; set; }   // Lista de evaluaciones
        public string? EvaluacionSeleccionada { get; set; }              // ID de la evaluación seleccionada
        public string? EvaluacionSeleccionadaDescripcion { get; set; }
    }

    public class EvaluacionViewModel
    {
        public int EvaluacionID { get; set; }            // ID de la evaluación
        public string? Evaluacion { get; set; }           // Nombre de la evaluación
        public byte? Estado { get; set; }                 // Tipo de evaluación
        public string? Sesiones { get; set; }
    }
}
