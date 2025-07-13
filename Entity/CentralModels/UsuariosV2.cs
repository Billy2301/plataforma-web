
namespace Entity.CentralModels;

public partial class UsuariosV2
{
    public int UsuarioId { get; set; }

    public byte SistemaId { get; set; }

    public int GrupoId { get; set; }

    public string? UsuarioNombre { get; set; }

    public string? Contrasena { get; set; }

    public bool Deshabilitado { get; set; }

    public bool EsSistema { get; set; }

    public string? ActualizadoPor { get; set; }

    public DateTime UltimaActualizacion { get; set; }

    public int EmpleadoId { get; set; }

    public int? CentroCosto2Id { get; set; }

    public int RefPersonalId { get; set; }

    public bool PuedeBorrarActa { get; set; }

    public bool PuedeBorrarConclu { get; set; }

    public bool PuedeRevertirEval { get; set; }

    public bool PuedeBorrarResumen { get; set; }

    public string? AgendaClave { get; set; }

    public string? UserEmail { get; set; }

    public int? RefHistoriaClinica { get; set; }

    public int? RefAlumnoId { get; set; }
}