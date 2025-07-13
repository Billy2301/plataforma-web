using Entity.CentralModels;
using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPagoExternoService
    {
        Task<PagosExterno> Guardar(PagosExterno entity);
        Task<PagosExterno> getPagoExterno(int pagoExtId);
        Task updatePagosDetalleReferencia(int pagoId, string referencia);
        Task NotificacionPago(PagosExterno entityPago, string bodyEmail, string email);
    }
}
