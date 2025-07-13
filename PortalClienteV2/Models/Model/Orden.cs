namespace PortalClienteV2.Models.Model
{
    public class Orden
    {
        public string? Object { get; set; }
        public string? OrderId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymenCode { get; set; }
        public string? Description { get; set; }
        public string? OrderNumber { get; set; }
        public Cliente? Customer { get; set; }
    }
}
