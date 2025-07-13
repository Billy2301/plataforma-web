using Entity.CentralModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IUbigeoService
    {
        Task<List<Pais>> ListaPaises();
        Task<List<Departamento>> ListaDepartamentosPorPaises(int CodPais);
        Task<List<Provincia>> ListaProvinciaPorDepartamento(int CodDepartamento);
        Task<List<Distrito>> ListaDistritoPorProvincia(int CodProvincia);

    }
}
