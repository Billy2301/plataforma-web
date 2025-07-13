namespace PortalClienteV2.Models.Model
{
    public class Agente
    {
        public string? rootApp { get; set; }
        public string? Url { get; set; }
        public bool isMobil { get; set; } = false;
        public string? ipAddress { get; set; }
        public string? userAgentString { get; set; }
        public string? userString { get; set; }
        public string? scheme { get; set; }
        public HostString? host { get; set; }
        public string? Controller { get; set; }
        public string? metod { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
    }
}
