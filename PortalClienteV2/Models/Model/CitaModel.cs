namespace PortalClienteV2.Models.Model
{
    public class CitaModel
    {
        public int PagoCitaDetalleId { get; set; }
        public int PagoCitaId { get; set; }
        public int EvaluacionAreaId { get; set; }
        public string EvaluacionAreaNombre { get; set; }
        public int EvaluacionId { get; set; }
        public string EvaluacionNombre { get; set; }
        public int EvaluacionCitaId { get; set; }
        public int NumeroCita { get; set; }
        public string NombreCita { get; set; }
        public decimal Precio { get; set; }
        public int TipoCitaId { get; set; }
        public string TipoCitaNombre { get; set; }
        public int Duracion { get; set; }
        public string? FechaCita { get; set; }
        public string? HoraCitaDesde { get; set; }
        public string? HoraCitaHasta { get; set; }
        public int? EspecialistaId { get; set; }
        public int SedeId { get; set; }
        public string SedeNombre { get; set; }
        public string ImagenUrl { get; set; }
        public bool IsProgramado { get; set; }
        public int CitaPara { get; set; }
    }
}
