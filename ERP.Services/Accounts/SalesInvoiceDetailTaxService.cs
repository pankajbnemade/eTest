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
    public class SalesInvoiceDetailTaxService : Repository<Salesinvoicedetailtax>, ISalesInvoiceDetailTax
    {
        public SalesInvoiceDetailTaxService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            int salesInvoiceDetailTaxId = 0;

            // assign values.
            Salesinvoicedetailtax salesInvoiceDetailTax = new Salesinvoicedetailtax();

            //salesInvoiceDetailTax.SalesInvoiceDetailTaxName = salesInvoiceDetailTaxModel.SalesInvoiceDetailTaxName;

            salesInvoiceDetailTaxId = await Create(salesInvoiceDetailTax);

            return salesInvoiceDetailTaxId; // returns.
        }


        public async Task<bool> UpdateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicedetailtax salesInvoiceDetailTax = await GetByIdAsync(w => w.InvoiceDetTaxId == salesInvoiceDetailTaxModel.InvoiceDetTaxId);

            if (null != salesInvoiceDetailTax)
            {
                // assign values.
                //salesInvoiceDetailTax.SalesInvoiceDetailTaxName = salesInvoiceDetailTaxModel.SalesInvoiceDetailTaxName;

                isUpdated = await Update(salesInvoiceDetailTax);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteSalesInvoiceDetailTax(int salesInvoiceDetailTaxId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicedetailtax salesInvoiceDetailTax = await GetByIdAsync(w => w.InvoiceDetTaxId == salesInvoiceDetailTaxId);

            if (null != salesInvoiceDetailTax)
            {
                isDeleted = await Delete(salesInvoiceDetailTax);
            }

            return isDeleted; // returns.
        }


        public async Task<SalesInvoiceDetailTaxModel> GetSalesInvoiceDetailTaxById(int salesInvoiceDetailTaxId)
        {
            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = null;

            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = await GetSalesInvoiceDetailTaxList(salesInvoiceDetailTaxId);

            if (null != salesInvoiceDetailTaxModelList && salesInvoiceDetailTaxModelList.Any())
            {
                salesInvoiceDetailTaxModel = salesInvoiceDetailTaxModelList.FirstOrDefault();
            }

            return salesInvoiceDetailTaxModel; // returns.
        }


        public async Task<IList<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxByStateId(int stateId)
        {
            return await GetSalesInvoiceDetailTaxList(0);
        }


        public async Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList()
        {
            DataTableResultModel<SalesInvoiceDetailTaxModel> resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();

            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = await GetSalesInvoiceDetailTaxList(0);

            if (null != salesInvoiceDetailTaxModelList && salesInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();
                resultModel.ResultList = salesInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = salesInvoiceDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList(int salesInvoiceDetailTaxId)
        {
            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = null;

            // create query.
            IQueryable<Salesinvoicedetailtax> query = GetQueryByCondition(w => w.InvoiceDetTaxId != 0);

            // apply filters.
            if (0 != salesInvoiceDetailTaxId)
                query = query.Where(w => w.InvoiceDetTaxId == salesInvoiceDetailTaxId);

          

            // get records by query.
            List<Salesinvoicedetailtax> salesInvoiceDetailTaxList = await query.ToListAsync();
            if (null != salesInvoiceDetailTaxList && salesInvoiceDetailTaxList.Count > 0)
            {
                salesInvoiceDetailTaxModelList = new List<SalesInvoiceDetailTaxModel>();

                foreach (Salesinvoicedetailtax salesInvoiceDetailTax in salesInvoiceDetailTaxList)
                {
                    salesInvoiceDetailTaxModelList.Add(await AssignValueToModel(salesInvoiceDetailTax));
                }
            }

            return salesInvoiceDetailTaxModelList; // returns.
        }

        private async Task<SalesInvoiceDetailTaxModel> AssignValueToModel(Salesinvoicedetailtax salesInvoiceDetailTax)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = new SalesInvoiceDetailTaxModel();

                //salesInvoiceDetailTaxModel.InvoiceDetTaxId = salesInvoiceDetailTax.InvoiceDetTaxId;
                //salesInvoiceDetailTaxModel.SalesInvoiceDetailTaxName = salesInvoiceDetailTax.SalesInvoiceDetailTaxName;

                return salesInvoiceDetailTaxModel;
            });
        }
    }
}
