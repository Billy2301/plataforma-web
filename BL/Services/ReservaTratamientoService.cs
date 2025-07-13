using BL.IServices;
using DA.IUOW;
using Entity.CentralModels;
using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ReservaTratamientoService : IReservaTratamientoService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;

        public ReservaTratamientoService(IUnitOfWorkClinica unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReservaTratamiento> Guardar(ReservaTratamiento entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<ReservaTratamiento>();
                ReservaTratamiento output;
                if (entity.ReservaTratamientoId == 0)
                {
                    output = await repository.Crear(entity);
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al actualizar datos");
                    }
                    output = entity;
                }
                _unitOfWork.SaveChanges();
                if (output.ReservaTratamientoId == 0)
                {
                    throw new InvalidOperationException("Error al guardar datos");
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }

        }

        public async Task<ReservaTratamiento?> GetReserva(int id)
        {
            var repository = _unitOfWork.GetRepository<ReservaTratamiento>();
            ReservaTratamiento? output = await repository.Obtener(p => p.ReservaTratamientoId == id);
            return output;
        }

        public async Task<DetalleReservaTratamiento> GuardarDetalle(DetalleReservaTratamiento entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<DetalleReservaTratamiento>();
                DetalleReservaTratamiento output;
                if (entity.DetalleReservaTratamientoId == 0)
                {
                    output = await repository.Crear(entity);
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al actualizar datos");
                    }
                    output = entity;
                }
                _unitOfWork.SaveChanges();
                if (output.DetalleReservaTratamientoId == 0)
                {
                    throw new InvalidOperationException("Error al guardar datos");
                }
                return output;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }


        }

        public async Task<List<UvReservaTratamientoSearch>> GetListReservaTratamientoPendienteAsync(string userId)
        {
            var repository = _unitOfWork.GetRepository<UvReservaTratamientoSearch>();
            var list = await repository.Consultar(t => t.UsuarioId == userId && (t.ReservaTratamientoEstadoId == 1) && t.DeleteDate == null);
            return list.ToList();
        }

        public async Task<List<UvReservaTratamientoSearch>> GetListReservaTratamientoPendienteTipoAsync(string userId, int tipo)
        {
            var repository = _unitOfWork.GetRepository<UvReservaTratamientoSearch>();
            var list = await repository.Consultar(t => t.UsuarioId == userId && (t.ReservaTratamientoEstadoId == 1) && t.Tipo == tipo && t.DeleteDate == null);
            return list.ToList();
        }

        public async Task<List<UvReservaTratamientoSearch>> GetListReservaTratamientoPendienteTipoWhitHistoriaAsync(string userId, int tipo, int hc)
        {
            var repository = _unitOfWork.GetRepository<UvReservaTratamientoSearch>();
            var list = await repository.Consultar(t => t.UsuarioId == userId && (t.ReservaTratamientoEstadoId == 1) && t.Tipo == tipo && t.DeleteDate == null && t.HistoriaClinica == hc);
            return list.ToList();
        }
        
    }
}
