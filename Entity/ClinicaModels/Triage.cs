
namespace Entity.ClinicaModels;

public partial class Triage
{
    public int TriageNo { get; set; }

    public DateTime Fecha { get; set; }

    public int? PersonalId { get; set; }

    public string? ApellidoPaternoPaciente { get; set; }

    public string? ApellidoMaternoPaciente { get; set; }

    public string? NombresPaciente { get; set; }

    public DateTime FechaNacimientoPaciente { get; set; }

    public byte SexoPaciente { get; set; }

    public string? TelefonoPaciente { get; set; }

    public DateTime? FechaEvaluacion { get; set; }

    public bool EvaluarNeurologia { get; set; }

    public bool EvaluarPsicologia { get; set; }

    public bool EvaluarLenguajeAprendizaje { get; set; }

    public bool EvaluarPsicomotriz { get; set; }

    public bool EvaluarAudiologia { get; set; }

    public bool EvaluarOrientacionVocacional { get; set; }

    public string? Observaciones { get; set; }

    public string? ActualizadoPor { get; set; }

    public DateTime UltimaActualizacion { get; set; }

    public string? Colegio { get; set; }

    public string? MotivoConsulta { get; set; }

    public short? TriageTipo { get; set; }

    public bool? EvaluarHabla { get; set; }

    public bool? EvaluaAprendizaje { get; set; }

    public bool? EvaluaMotividad { get; set; }

    public bool? EvaluaDisfluencia { get; set; }

    public bool? EvaluaVoz { get; set; }

    public byte? Comollegocpal { get; set; }

    public string? Descomollego { get; set; }

    public int? IdColegio { get; set; }

    public int? CodigoColegio { get; set; }

    public string? PacienteGrado { get; set; }

    public string? PacienteTel { get; set; }

    public string? PacienteProfNombre { get; set; }

    public string? PacienteProfTel { get; set; }

    public string? PacienteProfEmail { get; set; }

    public string? PacienteDerivadoPor { get; set; }

    public string? PacienteTratAnteriores { get; set; }

    public string? PacienteObservaciones { get; set; }

    public string? PacienteDireccion { get; set; }

    public short? PacienteCodigoPais { get; set; }

    public int? PacienteCodigoDepartamento { get; set; }

    public int? PacienteCodigoProvincia { get; set; }

    public int? PacienteCodigoDistrito { get; set; }

    public string? PacienteDomicilioTel { get; set; }

    public string? PacienteDomicilioTelEmer { get; set; }

    public string? PacienteDireccion2 { get; set; }

    public string? NombrePadre { get; set; }

    public string? OcupacionPadre { get; set; }

    public string? CentroTrabajoPadre { get; set; }

    public string? TelefonoPadre { get; set; }

    public string? CelularPadre { get; set; }

    public string? EmailPadre { get; set; }

    public string? NombreMadre { get; set; }

    public string? OcupacionMadre { get; set; }

    public string? CentroTrabajoMadre { get; set; }

    public string? TelefonoMadre { get; set; }

    public string? CelularMadre { get; set; }

    public string? EmaiMadre { get; set; }

    public short? NumeroHijosVarones { get; set; }

    public short? NumeroHijosMujeres { get; set; }

    public short? LugarEntreHermanos { get; set; }

    public string? PacienteViveCon { get; set; }

    public string? CreadoPor { get; set; }

    public DateTime? CreadoFecha { get; set; }

    public string? PacienteEmail { get; set; }

    public string? PacienteCentroEducativo { get; set; }

    public string? PacienteDni { get; set; }

    public bool? Evaluaudad { get; set; }

    public string? Presuncion { get; set; }

    public byte? Responsable { get; set; }

    public int? Tipo { get; set; }

    public int? Coordinarcoleg { get; set; }

    public bool? EvaluaTrastComunicacion { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int SedeId { get; set; }

    public string? EmailDeContacto { get; set; }

    public int? Usopublicidad { get; set; }

    public string? TipoDocumento { get; set; }

    public bool? Dependiente { get; set; }

    public byte? ContactoSexo { get; set; }

    public DateTime? ContactoFechaNac { get; set; }

    public string? ContactoTipoDoc { get; set; }

    public string? ContactoNroDoc { get; set; }

    public string? ConsultoPor { get; set; }

    public string? ApoderadoNombre { get; set; }

    public string? ApoderadoRelacion { get; set; }

    public string? ApoderadoTelCelular { get; set; }
}