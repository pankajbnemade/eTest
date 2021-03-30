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
    public class SalesInvoiceTaxService : Repository<Salesinvoicetax>, ISalesInvoiceTax
    {
        ISalesInvoice salesInvoice;

        public SalesInvoiceTaxService(ErpDbContext dbContext, ISalesInvoice _salesInvoice) : base(dbContext)
        {
            salesInvoice = _salesInvoice;
        }

        public async Task<int> CreateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel)
        {
            int salesInvoiceTaxId = 0;
            int multiplier = 1;

            // assign values.
            SalesInvoiceModel salesInvoiceModel = null;

            salesInvoiceModel = await salesInvoice.GetSalesInvoiceById((int)salesInvoiceTaxModel.InvoiceId);

            Salesinvoicetax salesInvoiceTax = new Salesinvoicetax();

            salesInvoiceTax.InvoiceId = salesInvoiceTaxModel.InvoiceId;
            salesInvoiceTax.SrNo = salesInvoiceTaxModel.SrNo;
            salesInvoiceTax.TaxLedgerId = salesInvoiceTaxModel.TaxLedgerId;
            salesInvoiceTax.TaxPercentageOrAmount = salesInvoiceTaxModel.TaxPercentageOrAmount;
            salesInvoiceTax.TaxPerOrAmountFc = salesInvoiceTaxModel.TaxPerOrAmountFc;

            if (salesInvoiceTax.TaxPercentageOrAmount == "Percentage")
            {
                salesInvoiceTaxModel.TaxAmountFc = (salesInvoiceModel.GrossAmountFc * salesInvoiceTaxModel.TaxPerOrAmountFc) / 100;
            }
            else
            {
                salesInvoiceTaxModel.TaxAmountFc = salesInvoiceTaxModel.TaxPerOrAmountFc;
            }

            if (salesInvoiceTaxModel.TaxAddOrDeduct == "Deduct")
            {
                multiplier = -1;
            }

            salesInvoiceTax.TaxAddOrDeduct = salesInvoiceTaxModel.TaxAddOrDeduct;
            salesInvoiceTax.TaxAmountFc = multiplier * salesInvoiceTaxModel.TaxAmountFc;
            salesInvoiceTax.TaxAmount = multiplier * salesInvoiceTaxModel.TaxAmount;
            salesInvoiceTax.Remark = salesInvoiceTaxModel.Remark;

            salesInvoiceTaxId = await Create(salesInvoiceTax);

            if (salesInvoiceTaxId != 0)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceTaxId);
            }

            return salesInvoiceTaxId; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Salesinvoicetax salesInvoiceTax = await GetByIdAsync(w => w.InvoiceTaxId == salesInvoiceTaxModel.InvoiceTaxId);

            if (null != salesInvoiceTax)
            {
                // assign values.

                salesInvoiceTax.InvoiceId = salesInvoiceTaxModel.InvoiceId;
                salesInvoiceTax.SrNo = salesInvoiceTaxModel.SrNo;
                salesInvoiceTax.TaxLedgerId = salesInvoiceTaxModel.TaxLedgerId;
                salesInvoiceTax.TaxPercentageOrAmount = salesInvoiceTaxModel.TaxPercentageOrAmount;
                salesInvoiceTax.TaxPerOrAmountFc = salesInvoiceTaxModel.TaxPerOrAmountFc;

                if (salesInvoiceTax.TaxPercentageOrAmount == "Percentage")
                {
                    salesInvoiceTaxModel.TaxAmountFc = (salesInvoiceTax.Invoice.GrossAmountFc * salesInvoiceTaxModel.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    salesInvoiceTaxModel.TaxAmountFc = salesInvoiceTaxModel.TaxPerOrAmountFc;
                }

                if (salesInvoiceTaxModel.TaxAddOrDeduct == "Deduct")
                {
                    multiplier = -1;
                }

                salesInvoiceTax.TaxAddOrDeduct = salesInvoiceTaxModel.TaxAddOrDeduct;
                salesInvoiceTax.TaxAmountFc = multiplier * salesInvoiceTaxModel.TaxAmountFc;
                salesInvoiceTax.TaxAmount = multiplier * salesInvoiceTaxModel.TaxAmount;
                salesInvoiceTax.Remark = salesInvoiceTaxModel.Remark;

                isUpdated = await Update(salesInvoiceTax);
            }

            if (isUpdated != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceTax.InvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceTax(int salesInvoiceTaxId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicetax salesInvoiceTax = await GetByIdAsync(w => w.InvoiceTaxId == salesInvoiceTaxId);

            if (null != salesInvoiceTax)
            {
                isDeleted = await Delete(salesInvoiceTax);
            }

            if (isDeleted != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceTax.InvoiceId);
            }

            return isDeleted; // returns.
        }

        public async Task<SalesInvoiceTaxModel> GetSalesInvoiceTaxById(int salesInvoiceTaxId)
        {
            SalesInvoiceTaxModel salesInvoiceTaxModel = null;

            IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList = await GetSalesInvoiceTaxList(salesInvoiceTaxId, 0);

            if (null != salesInvoiceTaxModelList && salesInvoiceTaxModelList.Any())
            {
                salesInvoiceTaxModel = salesInvoiceTaxModelList.FirstOrDefault();
            }

            return salesInvoiceTaxModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceTaxModel> resultModel = new DataTableResultModel<SalesInvoiceTaxModel>();

            IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList = await GetSalesInvoiceTaxList(0, salesInvoiceId);

            if (null != salesInvoiceTaxModelList && salesInvoiceTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceTaxModel>();
                resultModel.ResultList = salesInvoiceTaxModelList;
                resultModel.TotalResultCount = salesInvoiceTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList()
        {
            DataTableResultModel<SalesInvoiceTaxModel> resultModel = new DataTableResultModel<SalesInvoiceTaxModel>();

            IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList = await GetSalesInvoiceTaxList(0, 0);

            if (null != salesInvoiceTaxModelList && salesInvoiceTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceTaxModel>();
                resultModel.ResultList = salesInvoiceTaxModelList;
                resultModel.TotalResultCount = salesInvoiceTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceTaxModel>> GetSalesInvoiceTaxList(int salesInvoiceTaxId, int salesInvoiceId)
        {
            IList<SalesInvoiceTaxModel> salesInvoiceTaxModelList = null;

            // create query.
            IQueryable<Salesinvoicetax> query = GetQueryByCondition(w => w.InvoiceTaxId != 0);

            // apply filters.
            if (0 != salesInvoiceTaxId)
                query = query.Where(w => w.InvoiceTaxId == salesInvoiceTaxId);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.InvoiceId == salesInvoiceId);

            // get records by query.
            List<Salesinvoicetax> salesInvoiceTaxList = await query.ToListAsync();
            if (null != salesInvoiceTaxList && salesInvoiceTaxList.Count > 0)
            {
                salesInvoiceTaxModelList = new List<SalesInvoiceTaxModel>();

                foreach (Salesinvoicetax salesInvoiceTax in salesInvoiceTaxList)
                {
                    salesInvoiceTaxModelList.Add(await AssignValueToModel(salesInvoiceTax));
                }
            }

            return salesInvoiceTaxModelList; // returns.
        }

        private async Task<SalesInvoiceTaxModel> AssignValueToModel(Salesinvoicetax salesInvoiceTax)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceTaxModel salesInvoiceTaxModel = new SalesInvoiceTaxModel();

                salesInvoiceTaxModel.InvoiceTaxId = salesInvoiceTax.InvoiceTaxId;
                salesInvoiceTaxModel.InvoiceId = salesInvoiceTax.InvoiceId;
                salesInvoiceTaxModel.SrNo = salesInvoiceTax.SrNo;
                salesInvoiceTaxModel.TaxLedgerId = salesInvoiceTax.TaxLedgerId;
                salesInvoiceTaxModel.TaxPercentageOrAmount = salesInvoiceTax.TaxPercentageOrAmount;
                salesInvoiceTaxModel.TaxPerOrAmountFc = salesInvoiceTax.TaxPerOrAmountFc;
                salesInvoiceTaxModel.TaxAddOrDeduct = salesInvoiceTax.TaxAddOrDeduct;
                salesInvoiceTaxModel.TaxAmountFc = salesInvoiceTax.TaxAmountFc;
                salesInvoiceTaxModel.TaxAmount = salesInvoiceTax.TaxAmount;
                salesInvoiceTaxModel.Remark = salesInvoiceTax.Remark;

                salesInvoiceTaxModel.TaxLedgerName = salesInvoiceTax.TaxLedger.LedgerName;

                return salesInvoiceTaxModel;
            });
        }
    }
}
