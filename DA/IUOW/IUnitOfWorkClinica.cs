using DA.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace DA.IUOW
{
    public interface IUnitOfWorkClinica: IDisposable
    {
        IGenericRepositoryClinica<TEntity> GetRepository<TEntity>() where TEntity : class;
        void SaveChanges();
        Task SaveChangesAsync();
        DbTransaction BeginTransaction();
        void Commit();
        void Rollback();
        
    }
}
