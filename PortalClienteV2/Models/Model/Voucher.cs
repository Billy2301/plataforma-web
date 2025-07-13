namespace PortalClienteV2.Models.Model
{
    public class Voucher
    {
        public string? Banco { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? NroOperacion { get; set; }
        public string? Agencia { get; set; }
        public string? Moneda { get; set; }
        public string? NombreImagen { get; set; }
        public string? FileInternal { get; set; }
        public string? FileExternal { get; set; }
        public string? FileExtension { get; set; }


        public Cliente? Customer { get; set; }

    }
}
