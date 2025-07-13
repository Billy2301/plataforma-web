using Entity.ClinicaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.IServices
{
    public interface IEvaluacionCitasService
    {
        Task<List<UvEvaluacionCitaSearch>> ListEspecialidadEvaluacionCitas();
        Task<List<EvaluacionCitum>> getListEvaluacionCitaByEvaluacionId(int id);
        Task<List<UvEspecilidadesPublica>> getListEspecialidadesPublicas();
        Task<List<UvEvaluacionesPublica>> getListEvaluacionesPublicasByEsp(int id);
        Task<List<UvCitasPublica>> getListSesionesByEval(int id,int idPublic);
        Task<UvEvaluacionesPublica> GetEvaluacionPublicaById(int id);
        Task<List<EvaluacionCitum>> getListEvaluacionCitaByEvaluacionPublicaId(int id);
    }
}
