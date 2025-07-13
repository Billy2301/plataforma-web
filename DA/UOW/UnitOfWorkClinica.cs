using DA.ClinicaContext;
using DA.IRepository;
using DA.IUOW;
using DA.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;


namespace DA.UOW
{
    public class UnitOfWorkClinica : IUnitOfWorkClinica
    {
        private readonly CPALClinicaContext _context;
        private Dictionary<Type, object> _repositories;
        private DbTransaction _transaction;

        public UnitOfWorkClinica(CPALClinicaContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepositoryClinica<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                _repositories.Add(typeof(TEntity), new GenericRepositoryClinica<TEntity>(_context));
            }
            return (IGenericRepositoryClinica<TEntity>)_repositories[typeof(TEntity)];
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
        public DbTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction().GetDbTransaction();
        }
        public void Commit()
        {
            _transaction.Commit();
        }
        public void Rollback()
        {
            _transaction?.Rollback();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
