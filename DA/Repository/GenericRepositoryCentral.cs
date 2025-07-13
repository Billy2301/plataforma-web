using DA.CentralContext;
using DA.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace DA.Repository
{
    public class GenericRepositoryCentral<TEntity> : IGenericRepositoryCentral<TEntity> where TEntity : class
    {
        public readonly CPALCentralContext _dbContext;
        public GenericRepositoryCentral(CPALCentralContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TEntity?> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity? entidad = await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filtro);
                return entidad;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>>? filtro = null)
        {
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filtro).AsNoTracking();
            return queryEntidad;
        }
        public async Task ExecuteProcedure(string procedure)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(procedure);
        }
        public async Task ExecuteQueryWhitParameter(string sqlQuery, Dictionary<string, object>? parametros = null)
        {
            var parameterList = new List<SqlParameter>();

            if (parametros != null)
            {
                foreach (var parametro in parametros)
                {
                    // Agregar el parámetro al listado
                    var sqlParameter = new SqlParameter($"@{parametro.Key}", parametro.Value ?? DBNull.Value);

                    // Mapear tipos de datos específicos según el tipo del parámetro
                    if (parametro.Value is int) { sqlParameter.SqlDbType = SqlDbType.Int; }
                    else if (parametro.Value is bool) { sqlParameter.SqlDbType = SqlDbType.Bit; }
                    else if (parametro.Value is DateTime) { sqlParameter.SqlDbType = SqlDbType.DateTime; }
                    else if (parametro.Value is string) { sqlParameter.SqlDbType = SqlDbType.NVarChar; } // Puedes ajustar según la longitud necesaria}
                    else if (parametro.Value is float) { sqlParameter.SqlDbType = SqlDbType.Float; }
                    else if (parametro.Value is double) { sqlParameter.SqlDbType = SqlDbType.Float; } // O SqlDbType.Real, según la precisión necesaria
                    else if (parametro.Value is decimal) { sqlParameter.SqlDbType = SqlDbType.Decimal; }
                    else if (parametro.Value is byte) { sqlParameter.SqlDbType = SqlDbType.TinyInt; }
                    else if (parametro.Value is short) { sqlParameter.SqlDbType = SqlDbType.SmallInt; }
                    else if (parametro.Value is long) { sqlParameter.SqlDbType = SqlDbType.BigInt; }
                    else if (parametro.Value is byte[]) { sqlParameter.SqlDbType = SqlDbType.VarBinary; }
                    else if (parametro.Value is Guid) { sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier; }
                    // Puedes añadir más casos según sea necesario

                    parameterList.Add(sqlParameter);
                }
            }
            // Eliminar la última coma y cerrar la sentencia SQL
            sqlQuery = sqlQuery.TrimEnd(',') + ";";

            await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery, parameterList.ToArray());
        }
        public async Task<TEntity> EncontrarAsincrono(int id)
        {
            try
            {
                TEntity? output = await _dbContext.Set<TEntity>().FindAsync(id);
                return output;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public TEntity Encontrar(int id)
        {
            try
            {
                TEntity? output =  _dbContext.Set<TEntity>().Find(id);
                return output;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<TEntity>> ExecuteQueryAsync(string sqlQuery)
        {
            return await _dbContext.Set<TEntity>().FromSqlRaw(sqlQuery).ToListAsync();
        }
    }
}
