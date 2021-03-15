using ERP.DataAccess.EntityData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ErpDbContext _dbContext;

        public Repository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// create new record in entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            int id = await _dbContext.SaveChangesAsync();

            return id;
        }

        /// <summary>
        /// update existing record in entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Set<T>().Update(entity);
            int id = await _dbContext.SaveChangesAsync();
            return id == 0 ? false : true;
        }

        /// <summary>
        /// delete existing record from entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> Delete(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.Set<T>().Remove(entity);
            int id = await _dbContext.SaveChangesAsync();

            return id == 0 ? false : true;
        }

        /// <summary>
        /// get all records.
        /// </summary>
        /// <returns>
        /// return list of records.
        /// </returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// get first record based on condition.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get list of records based on condition.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// returns list of records.
        /// </returns>
        public async Task<IList<T>> GetListByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        /// <summary>
        /// create query to get records based on condition.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// returns query.
        /// </returns>
        public IQueryable<T> GetQueryByCondition(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(expression);
            return query;
        }

        /// <summary>
        /// check any record exists from entity based on condition.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return true if record exists.
        /// </returns>
        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().AnyAsync(expression);
        }
    }
}


// Scaffold-DbContext "server=127.0.0.1;user id=root;password=pgp_dev;port=3306;database=erpdb;" MySql.EntityFrameworkCore -OutputDir EntityModels -ContextDir EntityData -Context ErpDbContext -ContextNamespace ERP.DataAccess.EntityData -f

//Add-Migration DBInit

//Remove-Migration

//Update-Database
