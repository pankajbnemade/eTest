using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ERP.Services
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// create new record in entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return id.
        /// </returns>
        Task<int> Create(T entity);

        /// <summary>
        /// update existing record in entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> Update(T entity);

        /// <summary>
        /// delete existing record from entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> Delete(T entity);

        /// <summary>
        /// get all records.
        /// </summary>
        /// <returns>
        /// return list of records.
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// get first record based on condition.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// return record.
        /// </returns>
        Task<T> GetByIdAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// get list of records based on condition.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// returns list of records.
        /// </returns>
        Task<IList<T>> GetListByConditionAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// create query to get records based on condition.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>
        /// returns query.
        /// </returns>
        IQueryable<T> GetQueryByCondition(Expression<Func<T, bool>> expression);

        /// <summary>
        /// check any record exists from entity based on condition.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>
        /// return true if record exists.
        /// </returns>
        Task<bool> Any(Expression<Func<T, bool>> expression);
    }
}
