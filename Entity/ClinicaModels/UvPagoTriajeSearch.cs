
namespace Entity.ClinicaModels;

public partial class UvPagoTriajeSearch
{
    public int PagoCitaId { get; set; }

    public string? UsuarioId { get; set; }

    public int? HistoriaClinica { get; set; }

    public int? TriajeOnlineId { get; set; }

    public DateTime? CreadoFecha { get; set; }

    public DateTime? EliminadoFecha { get; set; }

    public string? EstadoPago { get; set; }

    public string? CreadoPor { get; set; }

    public string? ApellidoPaternoPaciente { get; set; }

    public string? ApellidoMaternoPaciente { get; set; }

    public string? NombresPaciente { get; set; }

    public string? NombreCompleto { get; set; }

    public DateTime FechaTriajeOnline { get; set; }
    public int? PagoTriajeId { get; set; }

    public DateTime? FechaPago { get; set; }

    public decimal? Monto { get; set; }

    public string? Descripcion { get; set; }
}