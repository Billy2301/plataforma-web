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
    public class ConfigSistemaService : IConfigSistemaService
    {
        private readonly IUnitOfWorkCentral _unitOfWork;
        public ConfigSistemaService(IUnitOfWorkCentral unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> getVariable(string configName)
        {
            var repository = _unitOfWork.GetRepository<UvConfiguracionVariable>();
            UvConfiguracionVariable output = await repository.Obtener(c => c.ConfigNombre == configName);
            return output.ConfigData.ToString();
        }
    }
}
