using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class TaxRegisterService : Repository<Taxregister>, ITaxRegister
    {
        public TaxRegisterService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateTaxRegister(TaxRegisterModel taxRegisterModel)
        {
            int taxRegisterId = 0;

            // assign values.
            Taxregister taxRegister = new Taxregister();

            //taxRegister.TaxRegisterName = taxRegisterModel.TaxRegisterName;

            taxRegisterId = await Create(taxRegister);

            return taxRegisterId; // returns.
        }


        public async Task<bool> UpdateTaxRegister(TaxRegisterModel taxRegisterModel)
        {
            bool isUpdated = false;

            // get record.
            Taxregister taxRegister = await GetByIdAsync(w => w.TaxRegisterId == taxRegisterModel.TaxRegisterId);

            if (null != taxRegister)
            {
                // assign values.
                //taxRegister.TaxRegisterName = taxRegisterModel.TaxRegisterName;

                isUpdated = await Update(taxRegister);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteTaxRegister(int taxRegisterId)
        {
            bool isDeleted = false;

            // get record.
            Taxregister taxRegister = await GetByIdAsync(w => w.TaxRegisterId == taxRegisterId);

            if (null != taxRegister)
            {
                isDeleted = await Delete(taxRegister);
            }

            return isDeleted; // returns.
        }


        public async Task<TaxRegisterModel> GetTaxRegisterById(int taxRegisterId)
        {
            TaxRegisterModel taxRegisterModel = null;

            IList<TaxRegisterModel> taxRegisterModelList = await GetTaxRegisterList(taxRegisterId);

            if (null != taxRegisterModelList && taxRegisterModelList.Any())
            {
                taxRegisterModel = taxRegisterModelList.FirstOrDefault();
            }

            return taxRegisterModel; // returns.
        }


        public async Task<IList<TaxRegisterModel>> GetTaxRegisterByStateId(int stateId)
        {
            return await GetTaxRegisterList(0);
        }


        public async Task<DataTableResultModel<TaxRegisterModel>> GetTaxRegisterList()
        {
            DataTableResultModel<TaxRegisterModel> resultModel = new DataTableResultModel<TaxRegisterModel>();

            IList<TaxRegisterModel> taxRegisterModelList = await GetTaxRegisterList(0);

            if (null != taxRegisterModelList && taxRegisterModelList.Any())
            {
                resultModel = new DataTableResultModel<TaxRegisterModel>();
                resultModel.ResultList = taxRegisterModelList;
                resultModel.TotalResultCount = taxRegisterModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<TaxRegisterModel>> GetTaxRegisterList(int taxRegisterId)
        {
            IList<TaxRegisterModel> taxRegisterModelList = null;

            // create query.
            IQueryable<Taxregister> query = GetQueryByCondition(w => w.TaxRegisterId != 0);

            // apply filters.
            if (0 != taxRegisterId)
                query = query.Where(w => w.TaxRegisterId == taxRegisterId);



            // get records by query.
            List<Taxregister> taxRegisterList = await query.ToListAsync();
            if (null != taxRegisterList && taxRegisterList.Count > 0)
            {
                taxRegisterModelList = new List<TaxRegisterModel>();

                foreach (Taxregister taxRegister in taxRegisterList)
                {
                    taxRegisterModelList.Add(await AssignValueToModel(taxRegister));
                }
            }

            return taxRegisterModelList; // returns.
        }

        private async Task<TaxRegisterModel> AssignValueToModel(Taxregister taxRegister)
        {
            return await Task.Run(() =>
            {
                TaxRegisterModel taxRegisterModel = new TaxRegisterModel();

                //taxRegisterModel.TaxRegisterId = taxRegister.TaxRegisterId;
                //taxRegisterModel.TaxRegisterName = taxRegister.TaxRegisterName;

                return taxRegisterModel;
            });
        }
    }
}
