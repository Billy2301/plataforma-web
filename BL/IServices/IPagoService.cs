using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IPagoService
    {
        Task<Pago> getPago(int pagoId);
    }
}
