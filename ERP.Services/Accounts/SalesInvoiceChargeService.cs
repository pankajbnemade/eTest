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
    public class SalesInvoiceChargeService : Repository<Salesinvoicecharge>, ISalesInvoiceCharge
    {
        public SalesInvoiceChargeService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateSalesInvoiceCharge(SalesInvoiceChargeModel salesInvoiceChargeModel)
        {
            int salesInvoiceChargeId = 0;

            // assign values.
            Salesinvoicecharge salesInvoiceCharge = new Salesinvoicecharge();

            //salesInvoiceCharge.SalesInvoiceChargeName = salesInvoiceChargeModel.SalesInvoiceChargeName;

            salesInvoiceChargeId = await Create(salesInvoiceCharge);

            return salesInvoiceChargeId; // returns.
        }


        public async Task<bool> UpdateSalesInvoiceCharge(SalesInvoiceChargeModel salesInvoiceChargeModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicecharge salesInvoiceCharge = await GetByIdAsync(w => w.InvoiceChargeId == salesInvoiceChargeModel.InvoiceChargeId);

            if (null != salesInvoiceCharge)
            {
                // assign values.
                //salesInvoiceCharge.SalesInvoiceChargeName = salesInvoiceChargeModel.SalesInvoiceChargeName;

                isUpdated = await Update(salesInvoiceCharge);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteSalesInvoiceCharge(int salesInvoiceChargeId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicecharge salesInvoiceCharge = await GetByIdAsync(w => w.InvoiceChargeId == salesInvoiceChargeId);

            if (null != salesInvoiceCharge)
            {
                isDeleted = await Delete(salesInvoiceCharge);
            }

            return isDeleted; // returns.
        }


        public async Task<SalesInvoiceChargeModel> GetSalesInvoiceChargeById(int salesInvoiceChargeId)
        {
            SalesInvoiceChargeModel salesInvoiceChargeModel = null;

            IList<SalesInvoiceChargeModel> salesInvoiceChargeModelList = await GetSalesInvoiceChargeList(salesInvoiceChargeId);

            if (null != salesInvoiceChargeModelList && salesInvoiceChargeModelList.Any())
            {
                salesInvoiceChargeModel = salesInvoiceChargeModelList.FirstOrDefault();
            }

            return salesInvoiceChargeModel; // returns.
        }


        public async Task<IList<SalesInvoiceChargeModel>> GetSalesInvoiceChargeByStateId(int stateId)
        {
            return await GetSalesInvoiceChargeList(0);
        }


        public async Task<DataTableResultModel<SalesInvoiceChargeModel>> GetSalesInvoiceChargeList()
        {
            DataTableResultModel<SalesInvoiceChargeModel> resultModel = new DataTableResultModel<SalesInvoiceChargeModel>();

            IList<SalesInvoiceChargeModel> salesInvoiceChargeModelList = await GetSalesInvoiceChargeList(0);

            if (null != salesInvoiceChargeModelList && salesInvoiceChargeModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceChargeModel>();
                resultModel.ResultList = salesInvoiceChargeModelList;
                resultModel.TotalResultCount = salesInvoiceChargeModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceChargeModel>> GetSalesInvoiceChargeList(int salesInvoiceChargeId)
        {
            IList<SalesInvoiceChargeModel> salesInvoiceChargeModelList = null;

            // create query.
            IQueryable<Salesinvoicecharge> query = GetQueryByCondition(w => w.InvoiceChargeId != 0);

            // apply filters.
            if (0 != salesInvoiceChargeId)
                query = query.Where(w => w.InvoiceChargeId == salesInvoiceChargeId);

          

            // get records by query.
            List<Salesinvoicecharge> salesInvoiceChargeList = await query.ToListAsync();
            if (null != salesInvoiceChargeList && salesInvoiceChargeList.Count > 0)
            {
                salesInvoiceChargeModelList = new List<SalesInvoiceChargeModel>();

                foreach (Salesinvoicecharge salesInvoiceCharge in salesInvoiceChargeList)
                {
                    salesInvoiceChargeModelList.Add(await AssignValueToModel(salesInvoiceCharge));
                }
            }

            return salesInvoiceChargeModelList; // returns.
        }

        private async Task<SalesInvoiceChargeModel> AssignValueToModel(Salesinvoicecharge salesInvoiceCharge)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceChargeModel salesInvoiceChargeModel = new SalesInvoiceChargeModel();

                //salesInvoiceChargeModel.InvoiceChargeId = salesInvoiceCharge.InvoiceChargeId;
                //salesInvoiceChargeModel.SalesInvoiceChargeName = salesInvoiceCharge.SalesInvoiceChargeName;

                return salesInvoiceChargeModel;
            });
        }
    }
}
