using BL.IServices;
using DA.CentralContext;
using DA.IUOW;
using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class PagoExternoVoucherService : IPagoExternoVoucherService
    {
        private readonly IUnitOfWorkCentral _unitOfWork;
        private readonly ICPALCentralContextProcedures _centralProcedures;

        public PagoExternoVoucherService(IUnitOfWorkCentral unitOfWork, ICPALCentralContextProcedures centralProcedures)
        {
            _unitOfWork = unitOfWork;
            _centralProcedures = centralProcedures;
        }

        public async Task<PagoExternoVoucher> Guardar(PagoExternoVoucher entity)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<PagoExternoVoucher>();
                if (entity.PagoExternoVoucherId == 0)
                {
                    PagoExternoVoucher entity_created = await repository.Crear(entity);
                    if (entity_created.PagoExternoVoucherId == 0)
                    {
                        throw new TaskCanceledException("Error al realizar el pago");
                    }
                    return entity_created;
                }
                else
                {
                    bool entity_updated = await repository.Editar(entity);
                    if (!entity_updated)
                    {
                        throw new TaskCanceledException("Error al realizar el pago");
                    }
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(ex.Message);
            }
        }

        public async Task updatePagosDetalleReferencia(int pagoId, string referencia)
        {
            await _centralProcedures.upV2UpdatePagoDetalleReferenciaAsync(pagoId, referencia);
        }

        public async Task updateFechaPagobyId(int pagoId, string fechaPago)
        {
            var repository = _unitOfWork.GetRepository<PagoExternoVoucher>();
            string query = string.Format("UPDATE Pagos SET FechaPago=CONVERT(date, '{0}') where pagoID={1}", fechaPago, pagoId);
            await repository.ExecuteProcedure(query);
        }
    }
}
