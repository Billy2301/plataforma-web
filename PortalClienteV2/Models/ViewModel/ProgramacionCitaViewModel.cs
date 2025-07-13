using Entity.ClinicaModels;

namespace PortalClienteV2.Models.ViewModel
{
    public class ProgramacionCitaViewModel
    {
        public int PagoCitaId { get; set; }
        public string? UsuarioId { get; set; }
        public int? HistoriaClinica { get; set; }
        public int? TriajeOnlineId { get; set; }
        public string? EstadoPago { get; set; }
        public int? PagoTriajeIdRef { get; set; }
        public DateTime? FechaNacPaciente { get; set; }
        public int? EdadPaciente
        {
            get
            {
                if (FechaNacPaciente == null)
                {
                    return null; // Si no hay fecha de nacimiento, no se puede calcular la edad
                }

                DateTime fechaActual = DateTime.Today;
                int edad = fechaActual.Year - FechaNacPaciente.Value.Year;

                // Verifica si el cumpleaños aún no ha pasado este año
                if (FechaNacPaciente.Value.Date > fechaActual.AddYears(-edad))
                {
                    edad--;
                }

                return edad;
            }
        }
        public DateTime? TiempoDeExpiracion { get; set; }
        public int? TiempoEsperaEnSegundos { get; set; }
        public string? AppName { get; set; }
        public List<UvPagoCitaDetalleSearch>? ListUvPagoCitaDetalle { get; set; }
    }
}
