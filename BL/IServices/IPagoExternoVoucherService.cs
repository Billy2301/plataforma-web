using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPagoExternoVoucherService
    {
        Task<PagoExternoVoucher> Guardar(PagoExternoVoucher entity);
        Task updatePagosDetalleReferencia(int pagoId, string referencia);
        Task updateFechaPagobyId(int pagoId, string fechaPago);
    }
}
