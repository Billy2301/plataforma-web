namespace PortalClienteV2.Models.Model
{
    public class Cargo
    {
        public string TokenId { get; set; }
        public string? Email { get; set; }
        public string? Card { get; set; }
        public string? Ip { get; set; }
        public string? Browser { get; set; }
        public string? Devicetype { get; set; }
        public int? Installments { get; set; }
        public string? Cardbrand { get; set; }
        public string? Cardtype { get; set; }
        public Cliente? Customer { get; set; }
    }
}
