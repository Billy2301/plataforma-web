using BL.IServices;
using DA.IUOW;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL.Services
{
    public class TriajeOnlineService : ITriajeOnlineService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;

        public TriajeOnlineService(IUnitOfWorkClinica unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Crear(TriageOnline modelTriaje, TriageCotejoExt modelCotejoExt, TriageCotejo modelCotejo)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if(modelTriaje.TriageOnlineId != 0)
                    {
                        var repository = _unitOfWork.GetRepository<TriageOnline>();
                        await repository.Editar(modelTriaje);
                        _unitOfWork.SaveChanges();
                        transaction.Commit();
                        return modelTriaje.TriageOnlineId;
                    }
                    else
                    {
                        var repositoryTriaje = _unitOfWork.GetRepository<TriageOnline>();
                        TriageOnline triaje = await repositoryTriaje.Crear(modelTriaje);
                        _unitOfWork.SaveChanges();

                        var repositoryCotejoExt = _unitOfWork.GetRepository<TriageCotejoExt>();
                        modelCotejoExt.TriageOnlineId = triaje.TriageOnlineId;
                        TriageCotejoExt cotejoExt = await repositoryCotejoExt.Crear(modelCotejoExt);

                        var repositoryCotejo = _unitOfWork.GetRepository<TriageCotejo>();
                        modelCotejo.TriageOnlineId = triaje.TriageOnlineId;
                        TriageCotejo cotejo = await repositoryCotejo.Crear(modelCotejo);

                        _unitOfWork.SaveChanges();

                        transaction.Commit();

                        return triaje.TriageOnlineId;
                    }                   
                }
                catch (DbUpdateException ex)
                {
                    // Manejar excepción específica de Entity Framework
                    transaction.Rollback();
                    // Loguear la excepción o manejarla de otra manera
                    throw;
                }
                catch (Exception ex)
                {
                    // Si algo falla, hacemos rollback de la transacción
                    transaction.Rollback();
                    throw ex; // O maneja la excepción de alguna otra manera
                }
            }

        }

        public async Task<UvPacienteTriajeSearch> GetTriageOnline(int id)
        {
            var repository = _unitOfWork.GetRepository<UvPacienteTriajeSearch>();
            UvPacienteTriajeSearch output = await repository.Obtener(p => p.TriageOnlineId == id);
            return output;
        }

        public async Task<String> SearchTriaje(string tipoDoc, string dni)
        {
            string output = "";
            var repository = _unitOfWork.GetRepository<Triage>();
            Triage? entity = await repository.Obtener(t => (t.TipoDocumento == tipoDoc || t.TipoDocumento == null) && t.PacienteDni == dni && t.DeletedDate == null);
            if(entity != null) { output = entity.TriageNo.ToString(); }
            return output;
        }

        public async Task<List<UvPacienteTriajeSearch>> GetListTriajeOnlinePendienteAsync(string userId)
        {
            var repository = _unitOfWork.GetRepository<UvPacienteTriajeSearch>();
            var list = await repository.Consultar(t => t.RefUsuarioId == userId && (t.TriageOnlineEstadoId == 1 || t.TriageOnlineEstadoId == 5) && t.EliminadoFecha == null);
            return list.ToList();
        }

        public async Task<List<UvReservaDiagnosticoSearch>> GetReservaDiagnosticoPendienteByDniAsync(string userId, string? dni)
        {
            var repository = _unitOfWork.GetRepository<UvReservaDiagnosticoSearch>();
            var list = await repository.Consultar(t => t.UsuarioId == userId && t.Documento == dni && t.EliminadoFecha == null);
            return list.ToList();
        }
        public async Task<List<UvReservaDiagnosticoSearch>> GetReservaDiagnosticoPendienteByDniAsync(string userId, string? dni, string estado)
        {
            var repository = _unitOfWork.GetRepository<UvReservaDiagnosticoSearch>();
            var list = await repository.Consultar(t => t.UsuarioId == userId && t.Documento == dni && t.EliminadoFecha == null && t.EstadoPago == estado);
            return list.ToList();
        }

        public async Task<TriageOnline> GetTriageOnlineById(int id)
        {
            var repository = _unitOfWork.GetRepository<TriageOnline>();
            TriageOnline output = await repository.Obtener(p => p.TriageOnlineId == id);
            return output;
        }

        public async Task<Triage> GetTriageById(int id)
        {
            var repository = _unitOfWork.GetRepository<Triage>();
            Triage output = await repository.Obtener(p => p.TriageNo == id);
            return output;
        }

        public async Task<Triage> GuardarTriage(Triage entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<Triage>();
                Triage output;
                if (entity.TriageNo == 0)
                {
                    int newid = await maxId() + 1;
                    entity.TriageNo = newid;
                    output = await repository.Crear(entity);
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al actualizar el triaje");
                    }
                    output = entity;
                }
                _unitOfWork.SaveChanges();
                if (output.TriageNo == 0)
                {
                    throw new InvalidOperationException("Error al guardar el triaje");
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }
        }

        public async Task<int> maxId()
        {
            var repository = _unitOfWork.GetRepository<Triage>();
            // Primero espera el resultado del método Consultar
            IQueryable<Triage> queryable = await repository.Consultar();
            // Luego aplica MaxAsync al IQueryable resultante
            int ultimoTriaje = await queryable.MaxAsync(x => (int?)x.TriageNo) ?? 0;
            // Retorna el maximo numero
            return ultimoTriaje;
        }

        public async Task<TriageOnline> GuardarTriageOnline(TriageOnline entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TriageOnline>();
                TriageOnline output;
                if (entity.TriageOnlineId == 0)
                {
                    output = await repository.Crear(entity);
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al actualizar el triajeOnline");
                    }
                    output = entity;
                }
                _unitOfWork.SaveChanges();
                if (output.TriageOnlineId == 0)
                {
                    throw new InvalidOperationException("Error al guardar el triajeOnline");
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }
        }

        public async Task<List<UvReservaDiagnosticoSearch>> GetReservaDiagnostico(string userId)
        {
            var repository = _unitOfWork.GetRepository<UvReservaDiagnosticoSearch>();
            var list = await repository.Consultar(t => t.UsuarioId == userId && t.EliminadoFecha == null);
            return list.ToList();
        }
    }
}
