using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPacienteService
    {
        Task<List<Paciente>> ListarPacientesPorUsuario(string userEmail);
        Task<Paciente?> GetPaciente(int nroHC);
        Task<List<upHistorialPacienteGetDiagV2Result>> getHCDiagnostico(string numeroHistoriaClinica);
        Task<List<upHistorialPacienteGetTratV2Result>> getHCTratamiento(string numeroHistoriaClinica);
        Task<List<upHistoriaClinicaGetTratamientoPUResult>> getHistorialCitaTrat(string numeroHistoriaClinica);
        Task<List<upHistoriaClinicaGetDiagnosticoV2Result>> getHistorialCitaDiag(string numeroHistoriaClinica);
        Task<Paciente?> GetPacienteByDni(string dni);
        Task<List<UvHistoriaDiagnosticoBusqueda2>> getHistorialDiagnosticoByHc(int numeroHistoriaClinica);
    }
}
