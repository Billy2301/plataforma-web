using Entity.ClinicaModels;

namespace PortalClienteV2.Models.ViewModel
{
    public class HistoriasViewModel
    {
        public int NroHistoriaClinica { get; set; }
        public List<Paciente>? ListPacientes { get; set; }
    }
}
