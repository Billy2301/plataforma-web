using BL.IServices;
using DA.CentralContext;
using DA.ClinicaContext;
using DA.IRepository;
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
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWorkCentral _unitOfWork;

        public PagoService(IUnitOfWorkCentral unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pago> getPago(int pagoId)
        {
            var repository = _unitOfWork.GetRepository<Pago>();
            Pago output = await repository.Obtener(p => p.PagoId == pagoId);
            return output;
        }
    }
}
