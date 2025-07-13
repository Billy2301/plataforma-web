namespace Entity.ClinicaModels;

public partial class TriageOnline
{
    public int TriageOnlineId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? EliminadoFecha { get; set; }

    public string? GuidCode { get; set; }

    public bool? GuidCheck { get; set; }

    public DateTime? GuidEndDate { get; set; }

    public int TriageOnlineEstadoId { get; set; }

    public string? EmailRegistro { get; set; }

    public int? TriageNoRef { get; set; }

    public int? PersonalId { get; set; }

    public string? ApellidoPaternoPaciente { get; set; }

    public string? ApellidoMaternoPaciente { get; set; }

    public string? NombresPaciente { get; set; }

    public DateTime? FechaNacimientoPaciente { get; set; }

    public byte? SexoPaciente { get; set; }

    public string? TelefonoPaciente { get; set; }

    public DateTime? FechaEvaluacion { get; set; }

    public bool EvaluarNeurologia { get; set; }

    public bool EvaluarPsicologia { get; set; }

    public bool EvaluarLenguajeAprendizaje { get; set; }

    public bool EvaluarPsicomotriz { get; set; }

    public bool EvaluarAudiologia { get; set; }

    public bool EvaluarOrientacionVocacional { get; set; }

    public string? Observaciones { get; set; }

    public string? Colegio { get; set; }

    public string? MotivoConsulta { get; set; }

    public short? TriageTipo { get; set; }

    public bool EvaluarHabla { get; set; }

    public bool EvaluaAprendizaje { get; set; }

    public bool EvaluaMotividad { get; set; }

    public bool EvaluaDisfluencia { get; set; }

    public bool EvaluaVoz { get; set; }

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

    public string? PacienteEmail { get; set; }

    public string? PacienteCentroEducativo { get; set; }

    public string? PacienteDni { get; set; }

    public bool? Evaluaudad { get; set; }

    public string? Presuncion { get; set; }

    public byte? Responsable { get; set; }

    public int? Tipo { get; set; }

    public int? Coordinarcoleg { get; set; }

    public bool? EvaluaTrastComunicacion { get; set; }

    public string? CorreoNombreDe { get; set; }

    public string? ParentescoPaciente { get; set; }

    public string? DireccionEnvio { get; set; }

    public short? PaisEnvio { get; set; }

    public int? DepartamenteEnvio { get; set; }

    public int? ProvinciaEnvio { get; set; }

    public int? DistritoEnvio { get; set; }

    public string? Procedencia { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? ActualizadoPor { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public int? TriageGenerado { get; set; }

    public string? EmailEvaluaciones { get; set; }

    public string? EmailProcedimiento1 { get; set; }

    public string? EmailProcedimiento2 { get; set; }

    public string? EmailProcedimiento3 { get; set; }

    public string? EmailBanco1 { get; set; }

    public string? EmailBanco2 { get; set; }

    public string? EmailBanco3 { get; set; }

    public string? EmailBanco4 { get; set; }

    public string? EmailBancoCuenta1 { get; set; }

    public string? EmailBancoCuenta2 { get; set; }

    public string? EmailBancoCuenta3 { get; set; }

    public string? EmailBancoCuenta4 { get; set; }

    public string? ApoderadoNombre { get; set; }

    public string? ApoderadoRelacion { get; set; }

    public string? ApoderadoTelFijo { get; set; }

    public string? ApoderadoTelCelular { get; set; }

    public string? ApoderadoEmail { get; set; }

    public int SedeId { get; set; }

    public string? EmailBancoCuentaCci1 { get; set; }

    public string? EmailBancoCuentaCci2 { get; set; }

    public string? EmailBancoCuentaCci3 { get; set; }

    public string? EmailBancoCuentaCci4 { get; set; }

    public string? TipoDocumento { get; set; }

    public bool? Dependiente { get; set; }

    public byte? ContactoSexo { get; set; }

    public DateTime? ContactoFechaNac { get; set; }

    public string? ContactoTipoDoc { get; set; }

    public string? ContactoNroDoc { get; set; }

    public string? ConsultoPor { get; set; }

    public string? LinkPago { get; set; }

    public int? RefNroHistoriaClinica { get; set; }

    public string? RefUsuarioId { get; set; }

    public string? DetalleOrientacion { get; set; }

    public bool? EsOrientado { get; set; }

    public byte? Area { get; set; }

    public virtual ICollection<PagoCita> PagoCitas { get; set; } = new List<PagoCita>();

    public virtual ICollection<TriageCotejoExt> TriageCotejoExts { get; set; } = new List<TriageCotejoExt>();

    public virtual ICollection<TriageCotejo> TriageCotejos { get; set; } = new List<TriageCotejo>();
}