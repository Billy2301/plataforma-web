using Entity.ClinicaModels;

namespace PortalClienteV2.Models.ViewModel
{
    public class PagoCitasViewModel
    {
        public List<UvPagoCitasSearch>? ListaPagoCitasSearch { get; set; }
        public int TipoComprobante { get; set; }
        public string NroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime? TiempoDeExpiracion { get; set; }
        public int? TiempoEsperaEnSegundos { get; set; }
    }
}
