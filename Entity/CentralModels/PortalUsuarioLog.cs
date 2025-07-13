
namespace Entity.CentralModels;

public partial class PortalUsuarioLog
{
    public int Id { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Nivel { get; set; }

    public string? Clase { get; set; }

    public string? Metodo { get; set; }

    public string? Mensaje { get; set; }

    public string? UsuarioId { get; set; }

    public string? UsuarioNombre { get; set; }

    public string? InformacionAdicional { get; set; }

    public string? Modulo { get; set; }

    public string? UsuarioIp { get; set; }

    public string? Browser { get; set; }
}