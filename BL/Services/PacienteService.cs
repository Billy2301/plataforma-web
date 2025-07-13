using BL.IServices;
using DA.ClinicaContext;
using DA.IUOW;
using Entity.ClinicaModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;
        private readonly ICPALClinicaContextProcedures _clinicaProcedure;

        public PacienteService(IUnitOfWorkClinica unitOfWork, ICPALClinicaContextProcedures clinicaProcedure)
        {
            _unitOfWork = unitOfWork;
            _clinicaProcedure = clinicaProcedure;
        }

        public async Task<List<Paciente>> ListarPacientesPorUsuario(string userEmail)
        {
            var repository = _unitOfWork.GetRepository<Paciente>();
            IQueryable<Paciente> outList = await repository.Consultar(p => p.DeletedDate == null && (p.EmailContacto == userEmail || p.EmailPadre == userEmail || p.EmaiMadre == userEmail));
            return await outList.ToListAsync();
        }

        public async Task<Paciente?> GetPaciente(int nroHC)
        {
            var repository = _unitOfWork.GetRepository<Paciente>();
            Paciente? output = await repository.Obtener(p => p.NumeroHistoriaClinica == nroHC);
            return output;
        }

        public async Task<List<upHistorialPacienteGetDiagV2Result>> getHCDiagnostico(string numeroHistoriaClinica)
        {
            List<upHistorialPacienteGetDiagV2Result> output = await _clinicaProcedure.upHistorialPacienteGetDiagV2Async(Convert.ToInt32(numeroHistoriaClinica));
            return output;
        }

        public async Task<List<upHistorialPacienteGetTratV2Result>> getHCTratamiento(string numeroHistoriaClinica)
        {
            List<upHistorialPacienteGetTratV2Result> output = await _clinicaProcedure.upHistorialPacienteGetTratV2Async(Convert.ToInt32(numeroHistoriaClinica));
            return output;
        }

        public async Task<List<upHistoriaClinicaGetDiagnosticoV2Result>> getHistorialCitaDiag(string numeroHistoriaClinica)
        {
            List<upHistoriaClinicaGetDiagnosticoV2Result> output = await _clinicaProcedure.upHistoriaClinicaGetDiagnosticoV2Async(Convert.ToInt32(numeroHistoriaClinica));
            return output;
        }

        public async Task<List<upHistoriaClinicaGetTratamientoPUResult>> getHistorialCitaTrat(string numeroHistoriaClinica)
        {
            List<upHistoriaClinicaGetTratamientoPUResult> output = await _clinicaProcedure.upHistoriaClinicaGetTratamientoPUAsync(Convert.ToInt32(numeroHistoriaClinica));
            return output;
        }

        public async Task<Paciente?> GetPacienteByDni(string dni)
        {
            var repository = _unitOfWork.GetRepository<Paciente>();
            Paciente? output = await repository.Obtener(p => p.PacienteDni == dni);
            return output;
        }

        public async Task<List<UvHistoriaDiagnosticoBusqueda2>> getHistorialDiagnosticoByHc(int numeroHistoriaClinica)
        {
            var repository = _unitOfWork.GetRepository<UvHistoriaDiagnosticoBusqueda2>();
            IQueryable<UvHistoriaDiagnosticoBusqueda2> output = await repository.Consultar(p => p.NumeroHistoriaClinica == numeroHistoriaClinica && p.HistorialEstadoId == 2 && p.TerapiaAsociada != null && p.FechaFin >= DateTime.Now.AddDays(-365));
            return output.ToList();
        }

    }
}
