using Entity.ClinicaModels;
using PortalClienteV2.Models.Model;

namespace PortalClienteV2.Models.ViewModel
{
    public class DiagnosticoViewModel
    {
        public List<upHistorialPacienteGetDiagV2Result>? ListaDiagnostico { get; set; }

        public int NroHc { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string Nombres { get; set; }
        public string PacienteDni { get; set; }
        public string EmailContacto { get; set; }
        public int IdPago { get; set; }
        public int TotalPagar { get; set; }
        public int TipoComprobante { get; set; }
        public string? NroDocumentoPago { get; set; }
        public string? RazonSocial { get; set; }
        public string? Direccion { get; set; }
        public Cargo Cargo { get; set; }
    }
}
