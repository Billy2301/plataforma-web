

namespace Entity.ClinicaModels;
public partial class UvPacienteTriajeSearch
{
    public int TriageOnlineId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? EliminadoFecha { get; set; }

    public string? EmailRegistro { get; set; }

    public int? TriageNoRef { get; set; }

    public string? ApellidoPaternoPaciente { get; set; }

    public string? ApellidoMaternoPaciente { get; set; }

    public string? NombresPaciente { get; set; }

    public DateTime? FechaNacimientoPaciente { get; set; }

    public byte? SexoPaciente { get; set; }

    public string? TelefonoPaciente { get; set; }

    public string? MotivoConsulta { get; set; }

    public string? PacienteGrado { get; set; }

    public string? PacienteDni { get; set; }

    public int SedeId { get; set; }

    public string? TipoDocumento { get; set; }

    public string? RefUsuarioId { get; set; }

    public int? RefNroHistoriaClinica { get; set; }

    public string? EmailBancoCuenta4 { get; set; }

    public string? Procedencia { get; set; }

    public string? ParentescoPaciente { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? ApoderadoNombre { get; set; }

    public string? ApoderadoEmail { get; set; }

    public int TriageOnlineEstadoId { get; set; }

    public string? TriageEstadoNombre { get; set; }
}