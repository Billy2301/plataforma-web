using Entity.ClinicaModels;

namespace PortalClienteV2.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<UvPagoTriajeSearch>? ListPagoTriaje { get; set; }
        public List<Paciente>? ListPacientes { get; set; }
        public List<UvPacienteTriajeSearch>? ListTriajeOnline { get; set; }
        public List<UvReservaTratamientoSearch>? ListReservaTx { get; set; }
        public List<UvReservaDiagnosticoSearch>? ListReservaDx { get; set; }
    }
}
