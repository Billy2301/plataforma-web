using BL.IServices;
using DA.IUOW;
using Entity.CentralModels;
using Entity.ClinicaModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class PagoCitaService : IPagoCitaService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;

        public PagoCitaService(IUnitOfWorkClinica unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PagoCita> getPagoCitaById(int pagoId)
        {
            var repository = _unitOfWork.GetRepository<PagoCita>();
            //PagoCita output = await repository.Obtener(p => p.PagoCitaId == pagoId,include: query => query.Include(pc => pc.PagoCitaDetalles));
            PagoCita output = await repository.Obtener(p => p.PagoCitaId == pagoId);
            return output;
        }
        public async Task<List<UvPagoCitaDetalleSearch>> getPagoCitaDetalle(int pagoCitaId)
        {
            var repository = _unitOfWork.GetRepository<UvPagoCitaDetalleSearch>();
            IQueryable<UvPagoCitaDetalleSearch> output = await repository.Consultar(p => p.PagoCitaId == pagoCitaId);
            return output.ToList();
        }
        public async Task<PagoCitaDetalle> getPagoCitaDetalleById(int id)
        {
            var repository = _unitOfWork.GetRepository<PagoCitaDetalle>();
            PagoCitaDetalle output = await repository.Obtener(p => p.PagoCitaDetalleId == id);
            return output;
        }
        public async Task<PagoCita> GuardarPagoCita(PagoCita entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<PagoCita>();
                PagoCita output;
                if (entity.PagoCitaId == 0)
                {
                    output = await repository.Crear(entity);
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al guardar el pago");
                    }
                    output =  entity;
                }
                _unitOfWork.SaveChanges();
                if (output.PagoCitaId == 0)
                {
                    throw new InvalidOperationException("Error al guardar la fecha de cita (crear).");
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }
        }
        public async Task<PagoCitaDetalle> GuardarPagoCitaDetalle(PagoCitaDetalle entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<PagoCitaDetalle>();
                PagoCitaDetalle output;
                if (entity.PagoCitaDetalleId == 0)
                {
                    output = await repository.Crear(entity);
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al guardar la fecha de cita");
                    }
                    output = entity;
                }
                _unitOfWork.SaveChanges();
                if (output.PagoCitaDetalleId == 0)
                {
                    throw new TaskCanceledException("Error al guardar el detalle");
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }
        }
        public async Task ExpirarCitasProgramadas(int id)
        {
            var repository = _unitOfWork.GetRepository<PagoCita>();
            await repository.ExecuteQuery($"EXEC sp_ExpirarCitas {id}");
        }
        public async Task<int> getNextNumeroCita(int pagoCitaId, int evaluacionId)
        {
            var repository = _unitOfWork.GetRepository<PagoCitaDetalle>();
            // Primero espera el resultado del método Consultar
            IQueryable<PagoCitaDetalle> queryable = await repository.Consultar(p => p.PagoCitaId == pagoCitaId && p.EvaluacionId == evaluacionId);
            // Luego aplica MaxAsync al IQueryable resultante
            int ultimoNumeroCita = await queryable.MaxAsync(x => (int?)x.NumeroCita) ?? 0;
            // Retorna el siguiente número consecutivo
            return ultimoNumeroCita + 1;
        }
        public async Task DeletePagoCitaDetalleByEvaluacion(int pagoCitaId, int evaluacionId)
        {
            var repository = _unitOfWork.GetRepository<PagoCita>();
            await repository.ExecuteQuery($"Delete PagoCitaDetalle where pagoCitaId = '{pagoCitaId}' and evaluacionId = '{evaluacionId}'");
        }
    }
}
