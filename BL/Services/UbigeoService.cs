using BL.IServices;
using DA.IRepository;
using DA.IUOW;
using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UbigeoService : IUbigeoService
    {
        private readonly IUnitOfWorkCentral _unitOfWork;


        public UbigeoService(IUnitOfWorkCentral unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Pais>> ListaPaises()
        {
            var repository = _unitOfWork.GetRepository<Pais>();
            IQueryable<Pais> query = await repository.Consultar();
            return query.ToList();
        }

        public async Task<List<Departamento>> ListaDepartamentosPorPaises(int CodPais)
        {
            var repository = _unitOfWork.GetRepository<Departamento>();
            IQueryable<Departamento> query = await repository.Consultar(d => d.CodigoPais == Convert.ToInt16(CodPais));
            return query.ToList();
        }

        public async Task<List<Provincia>> ListaProvinciaPorDepartamento(int CodDepartamento)
        {
            var repository = _unitOfWork.GetRepository<Provincia>();
            IQueryable<Provincia> query = await repository.Consultar(d => d.CodigoDepartamento == Convert.ToInt16(CodDepartamento));
            return query.ToList();
        }

        public async Task<List<Distrito>> ListaDistritoPorProvincia(int CodProvincia)
        {
            var repository = _unitOfWork.GetRepository<Distrito>();
            IQueryable<Distrito> query = await repository.Consultar(d => d.CodigoProvincia == Convert.ToInt16(CodProvincia));
            return query.ToList();
        }

    }
}
