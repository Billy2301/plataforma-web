namespace PortalClienteV2.Models.Model
{
    public class Cliente
    {
        public int? Pagoid { get; set; }
        public string? Area { get; set; }
        public int? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? Hc { get; set; }
        public string? Tipodocpago { get; set; }
        public string? Razonsocial { get; set; }
        public string? Ruc { get; set; }
        public string? Direccionpago { get; set; }
        public string? Email { get; set; }
        public int? TotalPagar { get; set; }
        public bool? ConDni { get; set; }
        public int? SedeId { get; set; }
        public string? TriajeOnLineId { get; set; }
        public string? Detalle { get; set; }
    }
}
