using BL.IServices;
using DA.IUOW;
using Entity.ClinicaModels;

namespace BL.Services
{
    public class EvaluacionCitasService : IEvaluacionCitasService
    {
        private readonly IUnitOfWorkClinica _unitOfWork;

        public EvaluacionCitasService(IUnitOfWorkClinica unitOfWorkClinica)
        {
                _unitOfWork = unitOfWorkClinica;
        }
        public async Task<List<UvEvaluacionCitaSearch>> ListEspecialidadEvaluacionCitas()
        {
            var repository = _unitOfWork.GetRepository<UvEvaluacionCitaSearch>();
            IQueryable<UvEvaluacionCitaSearch> query = await repository.Consultar();
            return query.ToList();
        }

        public async Task<List<EvaluacionCitum>> getListEvaluacionCitaByEvaluacionId(int id)
        {
            var repository = _unitOfWork.GetRepository<EvaluacionCitum>();
            IQueryable<EvaluacionCitum> query = await repository.Consultar(p => p.EvaluacionId == id);
            return query.ToList();
        }

        public async Task<List<UvEspecilidadesPublica>> getListEspecialidadesPublicas()
        {
            var repository = _unitOfWork.GetRepository<UvEspecilidadesPublica>();
            IQueryable<UvEspecilidadesPublica> query = await repository.Consultar();
            return query.ToList();
        }

        public async Task<List<UvEvaluacionesPublica>> getListEvaluacionesPublicasByEsp(int id)
        {
            var repository = _unitOfWork.GetRepository<UvEvaluacionesPublica>();
            IQueryable<UvEvaluacionesPublica> query = await repository.Consultar(e => e.EspecialidadId == id);
            return query.ToList();
        }

        public async Task<List<UvCitasPublica>> getListSesionesByEval(int id, int idPublic)
        {
            var repository = _unitOfWork.GetRepository<UvCitasPublica>();
            IQueryable<UvCitasPublica> query = await repository.Consultar(e => e.EvaluacionId == id && e.EvaluacionPublicaId == idPublic);
            return query.ToList();
        }

        public async Task<UvEvaluacionesPublica> GetEvaluacionPublicaById(int id)
        {
            var repository = _unitOfWork.GetRepository<UvEvaluacionesPublica>();
            UvEvaluacionesPublica output = await repository.Obtener(p => p.EvaluacionPublicaId == id);
            return output;
        }

        public async Task<List<EvaluacionCitum>> getListEvaluacionCitaByEvaluacionPublicaId(int id)
        {
            var repository = _unitOfWork.GetRepository<EvaluacionCitum>();
            IQueryable<EvaluacionCitum> query = await repository.Consultar(p => p.EvaluacionPublicaId == id);
            return query.ToList();
        }
    }
}
