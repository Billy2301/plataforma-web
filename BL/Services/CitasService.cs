using BL.IServices;
using DA.ClinicaContext;
using DA.IUOW;
using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CitasService : ICitasService
    {
        private readonly ICPALClinicaContextProcedures _clinicaProcedure;
        private readonly IUnitOfWorkClinica _unitOfWork;

        public CitasService(ICPALClinicaContextProcedures clinicaProcedure,IUnitOfWorkClinica unitOfWork)
        {
            _clinicaProcedure = clinicaProcedure;
            _unitOfWork = unitOfWork;
        }

        public async Task<IQueryable<upCitasProximasPorUsuarioResult>> getCitasProximas(string userName)
        {
            List<upCitasProximasPorUsuarioResult> list = await _clinicaProcedure.upCitasProximasPorUsuarioAsync(userName);
            var output = list.AsQueryable();
            return output;
        }

        public async Task<IQueryable<UvPagoCitasSearch>> getPagoCitaByDetalleIds(string ids)
        {
            var queryString = string.Format("SELECT * FROM [CPALClinica].[dbo].[uvPagoCitasSearch] where pagoCitaDetalleId in({0})", ids);

            var repository = _unitOfWork.GetRepository<UvPagoCitasSearch>();
            List<UvPagoCitasSearch> list = await repository.ExecuteQueryAsync(queryString);
            var output = list.AsQueryable();
            return output;
        }
    }
}
