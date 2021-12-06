using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Models.Helpers;

namespace ERP.Services.Master.Interface
{
    public interface ICompany : IRepository<Company>
    {

        /// <summary>
        /// create new company.
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns>
        /// return inserted companyId.
        /// </returns>
        Task<int> CreateCompany(CompanyModel companyModel);

        /// <summary>
        /// update company.
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> UpdateCompany(CompanyModel companyModel);

        /// <summary>
        /// update company.
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        Task<bool> DeleteCompany(int companyId);

        /// <summary>
        /// get company based on companyId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        Task<CompanyModel> GetCompanyById(int companyId);

        /// <summary>
        /// get all company list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<CompanyModel>> GetCompanyList();

        Task<IList<SelectListModel>> GetCompanySelectList();

        Task<IList<CompanyModel>> GetCompanyReportDataById(int companyId);

    }
}
