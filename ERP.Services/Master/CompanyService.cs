using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class CompanyService : Repository<Company>, ICompany
    {
        public CompanyService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateCompany(CompanyModel companyModel)
        {
            int companyId = 0;

            // assign values.
            Company company = new Company();

            company.CompanyName = companyModel.CompanyName;
            company.Address = companyModel.Address;
            company.EmailAddress = companyModel.EmailAddress;
            company.Website = companyModel.Website;
            company.PhoneNo = companyModel.PhoneNo;
            company.AlternatePhoneNo = companyModel.AlternatePhoneNo;
            company.FaxNo = companyModel.FaxNo;
            company.PostalCode = companyModel.PostalCode;
            company.CurrencyId = companyModel.CurrencyId;
            company.NoOfDecimals = companyModel.NoOfDecimals;

            companyId = await Create(company);

            return companyId; // returns.
        }

        /// <summary>
        /// update company.
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateCompany(CompanyModel companyModel)
        {
            bool isUpdated = false;

            // get record.
            Company company = await GetByIdAsync(w => w.CompanyId == companyModel.CompanyId);
            if (null != company)
            {
                // assign values.
                company.CompanyName = companyModel.CompanyName;
                company.Address = companyModel.Address;
                company.EmailAddress = companyModel.EmailAddress;
                company.Website = companyModel.Website;
                company.PhoneNo = companyModel.PhoneNo;
                company.AlternatePhoneNo = companyModel.AlternatePhoneNo;
                company.FaxNo = companyModel.FaxNo;
                company.PostalCode = companyModel.PostalCode;
                company.CurrencyId = companyModel.CurrencyId;
                company.NoOfDecimals = companyModel.NoOfDecimals;

                isUpdated = await Update(company);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// update company.
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteCompany(int companyId)
        {
            bool isDeleted = false;

            // get record.
            Company company = await GetByIdAsync(w => w.CompanyId == companyId);
            if (null != company)
            {
                isDeleted = await Delete(company);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get company based on companyId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<CompanyModel> GetCompanyById(int companyId)
        {
            CompanyModel companyModel = null;

            IList<CompanyModel> companyModelList = await GetCompanyList(companyId);
            if (null != companyModelList && companyModelList.Any())
            {
                companyModel = companyModelList.FirstOrDefault();
            }

            return companyModel; // returns.
        }


        /// <summary>
        /// get all company list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<CompanyModel>> GetCompanyList()
        {
            DataTableResultModel<CompanyModel> resultModel = new DataTableResultModel<CompanyModel>();

            IList<CompanyModel> companyModelList = await GetCompanyList(0);
            if (null != companyModelList && companyModelList.Any())
            {
                resultModel = new DataTableResultModel<CompanyModel>();
                resultModel.ResultList = companyModelList;
                resultModel.TotalResultCount = companyModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<CompanyModel>> GetCompanyList(int companyId)
        {
            IList<CompanyModel> companyModelList = null;

            // create query.
            IQueryable<Company> query = GetQueryByCondition(w => w.CompanyId != 0).Include(s => s.Currency).Include(s => s.PreparedByUser);

            // apply filters.
            if (0 != companyId)
                query = query.Where(w => w.CompanyId == companyId);

            // get records by query.
            List<Company> companyList = await query.ToListAsync();
            if (null != companyList && companyList.Count > 0)
            {
                companyModelList = new List<CompanyModel>();
                foreach (Company company in companyList)
                {
                    companyModelList.Add(await AssignValueToModel(company));
                }
            }

            return companyModelList; // returns.
        }

        private async Task<CompanyModel> AssignValueToModel(Company company)
        {
            return await Task.Run(() =>
            {
                CompanyModel companyModel = new CompanyModel();

                // assign values.
                Company company = new Company();

                companyModel.CompanyName = company.CompanyName;
                companyModel.Address = company.Address;
                companyModel.EmailAddress = company.EmailAddress;
                companyModel.Website = company.Website;
                companyModel.PhoneNo = company.PhoneNo;
                companyModel.AlternatePhoneNo = company.AlternatePhoneNo;
                companyModel.FaxNo = company.FaxNo;
                companyModel.PostalCode = company.PostalCode;
                companyModel.CurrencyId = (int)company.CurrencyId;
                companyModel.NoOfDecimals = (int)company.NoOfDecimals;

                companyModel.CurrencyName = company.Currency.CurrencyName;
                companyModel.PreparedByName = company.PreparedByUser.UserName;

                return companyModel;
            });
        }

        public async Task<IList<SelectListModel>> GetCompanySelectList()
        {
            IList<SelectListModel> resultModel = null;

            if (await Any(w => w.CompanyId != 0))
            {
                IQueryable<Company> query = GetQueryByCondition(w => w.CompanyId != 0);

                resultModel = await query
                                    .Select(s => new SelectListModel
                                    {
                                        DisplayText = s.CompanyName,
                                        Value = s.CompanyId.ToString()
                                    })
                                    .ToListAsync();
            }

            return resultModel; // returns.
        }

    }
}
