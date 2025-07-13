using DA.IRepository;
using System.Data.Common;

namespace DA.IUOW
{
    public interface IUnitOfWorkCentral : IDisposable
    {
        IGenericRepositoryCentral<TEntity> GetRepository<TEntity>() where TEntity : class;
        void SaveChanges();
        DbTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
