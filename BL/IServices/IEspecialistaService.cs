using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IEspecialistaService
    {
        Task<List<upAgendaDiagnosticoV2Result>> GetAgendaDiagnostico(int personalID, DateTime dateFrom, DateTime dateTo, int sedeID);
        Task<List<PersonalHorario>> GetEspecialistaHorario(int personalId);
        Task<List<UvPersonalEvaluacionSearch>> GetPersonalEvaluacionByRangoEdad(int evaluacionId, int? rangoId, int personalId);
        Task<List<upCalendarioDisponibleDiagV2Result>> GetCalendarioDisponibleDiagnostico(DateTime dateInicio , int especialidadID , int rangoEdadID , int personalID , int sedeID );
        Task<List<upCalendarioDisponibleDiagV3PortalResult>> GetCalendarioDisponibleDiagnosticoPortal(DateTime fechaSeleccionada, int especialidadID, int rangoEdadID, int personalID, int sedeID, int diasProximos, int tipo, int duracion);
        Task<List<upCalendarioDisponibleDiagPresencialResult>> GetCalendarioDisponibleDiagnosticoPresencial(int evaluacionID, int rangoEdadID, int personalID, int sedeID, int diasProximos, int duracion);
        Task<List<upCalendarioDisponibleDiagVirtualResult>> GetCalendarioDisponibleDiagnosticoVirtual(int evaluacionID, int rangoEdadID, int personalID, int sedeID, int diasProximos, int duracion);
    }
}
