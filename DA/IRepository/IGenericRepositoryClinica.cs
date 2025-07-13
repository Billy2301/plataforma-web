using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DA.IRepository
{
    public interface IGenericRepositoryClinica<TEntity> where TEntity : class
    {
        Task<TEntity?> Obtener(Expression<Func<TEntity, bool>> filtro);
        Task<TEntity?> Obtener(Expression<Func<TEntity, bool>> filtro, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
        Task<TEntity> Crear(TEntity entidad);
        Task<bool> Editar(TEntity entidad);
        Task<bool> Eliminar(TEntity entidad);
        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>>? filtro = null);
        Task ExecuteQuery(string procedure);
        Task ExecuteQueryWhitParameter(string sqlQuery, Dictionary<string, object>? parametros = null);
        Task<TEntity> EncontrarAsincrono(int id);
        TEntity Encontrar(int id);
        Task<List<TEntity>> ExecuteQueryAsync(string sqlQuery);
    }
}
