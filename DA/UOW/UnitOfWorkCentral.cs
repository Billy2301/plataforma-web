using DA.CentralContext;
using DA.ClinicaContext;
using DA.IRepository;
using DA.IUOW;
using DA.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.UOW
{
    public class UnitOfWorkCentral : IUnitOfWorkCentral
    {
        private readonly CPALCentralContext _context;
        private Dictionary<Type, object> _repositories;
        private DbTransaction _transaction;

        public UnitOfWorkCentral(CPALCentralContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepositoryCentral<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                _repositories.Add(typeof(TEntity), new GenericRepositoryCentral<TEntity>(_context));
            }
            return (IGenericRepositoryCentral<TEntity>)_repositories[typeof(TEntity)];
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
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
