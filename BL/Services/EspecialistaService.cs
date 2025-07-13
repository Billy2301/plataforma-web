using BL.IServices;
using DA.ClinicaContext;
using DA.IUOW;
using Entity.ClinicaModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL.Services
{
    public class EspecialistaService : IEspecialistaService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;
        private readonly ICPALClinicaContextProcedures _clinicaProcedures;

        public EspecialistaService(IUnitOfWorkClinica unitOfWork, ICPALClinicaContextProcedures clinicaProcedures)
        {
            _unitOfWork = unitOfWork;
            _clinicaProcedures = clinicaProcedures;
        }

        public async Task<List<upAgendaDiagnosticoV2Result>> GetAgendaDiagnostico(int personalID, DateTime dateFrom, DateTime dateTo, int sedeID)
        {
            List<upAgendaDiagnosticoV2Result> outList = await _clinicaProcedures.upAgendaDiagnosticoV2Async(personalID, dateFrom, dateTo, sedeID);
            return outList;
        }

        public async Task<List<upCalendarioDisponibleDiagV2Result>> GetCalendarioDisponibleDiagnostico(DateTime dateInicio, int especialidadID, int rangoEdadID, int personalID, int sedeID)
        {
            List<upCalendarioDisponibleDiagV2Result> outList = await _clinicaProcedures.upCalendarioDisponibleDiagV2Async(dateInicio, especialidadID, rangoEdadID, personalID, sedeID); 
            return outList;
        }

        public async Task<List<PersonalHorario>> GetEspecialistaHorario(int personalId)
        {
            var repository = _unitOfWork.GetRepository<PersonalHorario>();
            IQueryable<PersonalHorario> outList = await repository.Consultar(p => p.PersonalId == personalId && p.PeriodoInicio != null && p.PeriodoFin == null);
            return await outList.ToListAsync();
        }

        public async Task<List<UvPersonalEvaluacionSearch>> GetPersonalEvaluacionByRangoEdad(int evaluacionId, int? rangoId, int personalId)
        {
            var repository = _unitOfWork.GetRepository<UvPersonalEvaluacionSearch>();
            IQueryable<UvPersonalEvaluacionSearch> outList = await repository.Consultar(p => p.EvaluacionId == evaluacionId && p.RangoEdadAtencionId == rangoId && p.PersonalId == (personalId == 0 ? p.PersonalId : personalId));
            return outList.ToList();
        }

        public async Task<List<upCalendarioDisponibleDiagV3PortalResult>> GetCalendarioDisponibleDiagnosticoPortal(DateTime fechaSeleccionada,int especialidadID, int rangoEdadID, int personalID, int sedeID,int diasProximos, int tipo, int duracion)
        {
            List<upCalendarioDisponibleDiagV3PortalResult> outList = await _clinicaProcedures.upCalendarioDisponibleDiagV3PortalAsync(fechaSeleccionada, especialidadID, rangoEdadID, personalID, sedeID, diasProximos, tipo, duracion);
            return outList;
        }

        public async Task<List<upCalendarioDisponibleDiagPresencialResult>> GetCalendarioDisponibleDiagnosticoPresencial(int evaluacionID, int rangoEdadID, int personalID, int sedeID, int diasProximos, int duracion)
        {
            List<upCalendarioDisponibleDiagPresencialResult> outList = await _clinicaProcedures.upCalendarioDisponibleDiagPresencialAsync(evaluacionID, rangoEdadID, personalID, sedeID, diasProximos, duracion);
            return outList;
        }

        public async Task<List<upCalendarioDisponibleDiagVirtualResult>> GetCalendarioDisponibleDiagnosticoVirtual(int evaluacionID, int rangoEdadID, int personalID, int sedeID, int diasProximos, int duracion)
        {
            List<upCalendarioDisponibleDiagVirtualResult> outList = await _clinicaProcedures.upCalendarioDisponibleDiagVirtualAsync(evaluacionID, rangoEdadID, personalID, sedeID, diasProximos, duracion);
            return outList;
        }
    }
}
