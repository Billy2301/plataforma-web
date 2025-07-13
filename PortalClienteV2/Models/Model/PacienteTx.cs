using Entity.ClinicaModels;

namespace PortalClienteV2.Models.Model
{
    public class PacienteTx
    {
        public string? Hc { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Existe { get; set; }
        public List<UvHistoriaDiagnosticoBusqueda2>? listInforme { get; set; }
    }
}
