namespace PortalClienteV2.Models.ViewModel
{
    public class EvaluacionPagoViewModel
    {
        public int PagoCitaId { get; set; }
        public DateTime? CreadoFecha { get; set; }
        public string EstadoPago { get; set; }
        public int? EvaluacionAreaId { get; set; }
        public int? EvaluacionId { get; set; }
        public string Nombre { get; set; }
        public int? EspecialistaId { get; set; }
        public int? SedeId { get; set; }
        public string Sede { get; set; }
        public string ImagenUrl { get; set; }
        public int? TotalSesiones { get; set; }
        public decimal? Precio { get; set; }
    }
}
