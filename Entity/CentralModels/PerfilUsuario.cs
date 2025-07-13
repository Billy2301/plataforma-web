namespace Entity.CentralModels;

public partial class PerfilUsuario
{
    public int Id { get; set; }

    public string? Nombres { get; set; }

    public string? ApellidoPaterno { get; set; }
    public string? ApellidoMaterno { get; set; }

    public byte? Sexo { get; set; }
    public int? Pais { get; set; }

    public int? Region { get; set; }

    public int? Departamento { get; set; }

    public string? Dirección { get; set; }

    public string? Telefono { get; set; }

    public string? TipoDoc { get; set; }

    public string? Dni { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? ImagenInternal { get; set; }

    public string? ImagenExternal { get; set; }

    public string? ImagenExtension { get; set; }

    public string? UrlImagen { get; set; }

    public string? ActualizadoPor { get; set; }

    public DateTime? CreadoFecha { get; set; }

    public string? RefAspNetUser { get; set; }
}