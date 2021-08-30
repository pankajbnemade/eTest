using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class SalesInvoiceDetailTaxService : Repository<Salesinvoicedetailtax>, ISalesInvoiceDetailTax
    {
        ISalesInvoiceDetail salesInvoiceDetail;

        public SalesInvoiceDetailTaxService(ErpDbContext dbContext,  ISalesInvoiceDetail _salesInvoiceDetail) : base(dbContext)
        {
            salesInvoiceDetail = _salesInvoiceDetail;
        }

        /// <summary>
        /// generate sr no based on salesInvoiceDetId
        /// </summary>
        /// <param name="salesInvoiceDetId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int salesInvoiceDetId)
        {
            int srNo = 0;

            if (await Any(w => w.SalesInvoiceDetTaxId != 0 && w.SalesInvoiceDetId == salesInvoiceDetId))
            {
                srNo = await GetQueryByCondition(w => w.SalesInvoiceDetTaxId != 0 && w.SalesInvoiceDetId == salesInvoiceDetId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            int salesInvoiceDetailTaxId = 0;
            int multiplier = 1;

            SalesInvoiceDetailModel salesInvoiceDetailModel = null;
            salesInvoiceDetailModel = await salesInvoiceDetail.GetSalesInvoiceDetailById((Int32)salesInvoiceDetailTaxModel.SalesInvoiceDetId);

            // assign values.
            Salesinvoicedetailtax salesInvoiceDetailTax = new Salesinvoicedetailtax();

            salesInvoiceDetailTax.SalesInvoiceDetId = salesInvoiceDetailTaxModel.SalesInvoiceDetId;
            salesInvoiceDetailTax.SrNo = salesInvoiceDetailTaxModel.SrNo;
            salesInvoiceDetailTax.TaxLedgerId = salesInvoiceDetailTaxModel.TaxLedgerId;
            salesInvoiceDetailTax.TaxPercentageOrAmount = salesInvoiceDetailTaxModel.TaxPercentageOrAmount;
            salesInvoiceDetailTax.TaxPerOrAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;

            if (DiscountType.Percentage.ToString() == salesInvoiceDetailTaxModel.TaxPercentageOrAmount)
            {
                salesInvoiceDetailTaxModel.TaxAmountFc = (salesInvoiceDetailModel.GrossAmountFc * salesInvoiceDetailTaxModel.TaxPerOrAmountFc) / 100;
            }
            else
            {
                salesInvoiceDetailTaxModel.TaxAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;
            }

            if (TaxAddOrDeduct.Deduct.ToString() == salesInvoiceDetailTaxModel.TaxAddOrDeduct)
            {
                multiplier = -1;
            }

            salesInvoiceDetailTax.TaxAddOrDeduct = salesInvoiceDetailTaxModel.TaxAddOrDeduct;
            salesInvoiceDetailTax.TaxAmountFc = multiplier * salesInvoiceDetailTaxModel.TaxAmountFc;
            salesInvoiceDetailTax.TaxAmount = multiplier * salesInvoiceDetailTaxModel.TaxAmount;
            salesInvoiceDetailTax.Remark = salesInvoiceDetailTaxModel.Remark;

            await Create(salesInvoiceDetailTax);

            salesInvoiceDetailTaxId = salesInvoiceDetailTax.SalesInvoiceDetTaxId;

            //salesInvoiceDetailTaxId = await Create(salesInvoiceDetailTax);

            if (salesInvoiceDetailTaxId != 0)
            {
                await salesInvoiceDetail.UpdateSalesInvoiceDetailAmount(salesInvoiceDetailTax.SalesInvoiceDetId);
            }

            return salesInvoiceDetailTaxId; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Salesinvoicedetailtax salesInvoiceDetailTax = await GetQueryByCondition(w => w.SalesInvoiceDetTaxId == salesInvoiceDetailTaxModel.SalesInvoiceDetTaxId)
                                                                .Include(w => w.SalesInvoiceDet).FirstOrDefaultAsync();

            if (null != salesInvoiceDetailTax)
            {
                // assign values.
                salesInvoiceDetailTax.SalesInvoiceDetId = salesInvoiceDetailTaxModel.SalesInvoiceDetId;
                salesInvoiceDetailTax.SrNo = salesInvoiceDetailTaxModel.SrNo;
                salesInvoiceDetailTax.TaxLedgerId = salesInvoiceDetailTaxModel.TaxLedgerId;
                salesInvoiceDetailTax.TaxPercentageOrAmount = salesInvoiceDetailTaxModel.TaxPercentageOrAmount;
                salesInvoiceDetailTax.TaxPerOrAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;

                if (DiscountType.Percentage.ToString() == salesInvoiceDetailTaxModel.TaxPercentageOrAmount)
                {
                    salesInvoiceDetailTaxModel.TaxAmountFc = (salesInvoiceDetailTax.SalesInvoiceDet.GrossAmountFc * salesInvoiceDetailTaxModel.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    salesInvoiceDetailTaxModel.TaxAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == salesInvoiceDetailTaxModel.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                salesInvoiceDetailTax.TaxAddOrDeduct = salesInvoiceDetailTaxModel.TaxAddOrDeduct;
                salesInvoiceDetailTax.TaxAmountFc = multiplier * salesInvoiceDetailTaxModel.TaxAmountFc;
                salesInvoiceDetailTax.TaxAmount = multiplier * salesInvoiceDetailTaxModel.TaxAmount;
                salesInvoiceDetailTax.Remark = salesInvoiceDetailTaxModel.Remark;
                isUpdated = await Update(salesInvoiceDetailTax);
            }

            if (isUpdated != false)
            {
                await salesInvoiceDetail.UpdateSalesInvoiceDetailAmount(salesInvoiceDetailTax.SalesInvoiceDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceDetailTax(int salesInvoiceDetailTaxId)
        {
            bool isDeleted = false;

            // get record.

            Salesinvoicedetailtax salesInvoiceDetailTax = await GetQueryByCondition(w => w.SalesInvoiceDetTaxId == salesInvoiceDetailTaxId)
                                                                .FirstOrDefaultAsync();

            if (null != salesInvoiceDetailTax)
            {
                isDeleted = await Delete(salesInvoiceDetailTax);
            }

            if (isDeleted != false)
            {
                await salesInvoiceDetail.UpdateSalesInvoiceDetailAmount(salesInvoiceDetailTax.SalesInvoiceDetId);
            }

            return isDeleted; // returns.
        }

        public async Task<SalesInvoiceDetailTaxModel> GetSalesInvoiceDetailTaxById(int salesInvoiceDetailTaxId)
        {
            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = null;

            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = await GetSalesInvoiceDetailTaxList(salesInvoiceDetailTaxId, 0, 0);

            if (null != salesInvoiceDetailTaxModelList && salesInvoiceDetailTaxModelList.Any())
            {
                salesInvoiceDetailTaxModel = salesInvoiceDetailTaxModelList.FirstOrDefault();
            }

            return salesInvoiceDetailTaxModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxBySalesInvoiceDetailId(int salesInvoiceDetailId)
        {
            DataTableResultModel<SalesInvoiceDetailTaxModel> resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();

            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = await GetSalesInvoiceDetailTaxList(0, salesInvoiceDetailId, 0);
            if (null != salesInvoiceDetailTaxModelList && salesInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();
                resultModel.ResultList = salesInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = salesInvoiceDetailTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();
                resultModel.ResultList = new List<SalesInvoiceDetailTaxModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList()
        {
            DataTableResultModel<SalesInvoiceDetailTaxModel> resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();

            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = await GetSalesInvoiceDetailTaxList(0, 0, 0);
            if (null != salesInvoiceDetailTaxModelList && salesInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailTaxModel>();
                resultModel.ResultList = salesInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = salesInvoiceDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceDetailTaxModel>> GetSalesInvoiceDetailTaxList(int salesInvoiceDetailTaxId, int salesInvoiceDetailId, int salesInvoiceId)
        {
            IList<SalesInvoiceDetailTaxModel> salesInvoiceDetailTaxModelList = null;

            // create query.
            IQueryable<Salesinvoicedetailtax> query = GetQueryByCondition(w => w.SalesInvoiceDetTaxId != 0).Include(w => w.TaxLedger);
            // apply filters.
            if (0 != salesInvoiceDetailTaxId)
                query = query.Where(w => w.SalesInvoiceDetTaxId == salesInvoiceDetailTaxId);

            // apply filters.
            if (0 != salesInvoiceDetailId)
                query = query.Where(w => w.SalesInvoiceDetId == salesInvoiceDetailId);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.SalesInvoiceDet.SalesInvoiceId == salesInvoiceId);

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
                salesInvoiceDetailTaxModel.SalesInvoiceDetTaxId = salesInvoiceDetailTax.SalesInvoiceDetTaxId;
                salesInvoiceDetailTaxModel.SalesInvoiceDetId = salesInvoiceDetailTax.SalesInvoiceDetId;
                salesInvoiceDetailTaxModel.SrNo = salesInvoiceDetailTax.SrNo;
                salesInvoiceDetailTaxModel.TaxLedgerId = salesInvoiceDetailTax.TaxLedgerId;
                salesInvoiceDetailTaxModel.TaxPercentageOrAmount = salesInvoiceDetailTax.TaxPercentageOrAmount;
                salesInvoiceDetailTaxModel.TaxPerOrAmountFc = salesInvoiceDetailTax.TaxPerOrAmountFc;
                salesInvoiceDetailTaxModel.TaxAddOrDeduct = salesInvoiceDetailTax.TaxAddOrDeduct;
                salesInvoiceDetailTaxModel.TaxAmountFc = salesInvoiceDetailTax.TaxAmountFc;
                salesInvoiceDetailTaxModel.TaxAmount = salesInvoiceDetailTax.TaxAmount;
                salesInvoiceDetailTaxModel.Remark = salesInvoiceDetailTax.Remark;
                // ###
                salesInvoiceDetailTaxModel.TaxLedgerName = null != salesInvoiceDetailTax.TaxLedger ? salesInvoiceDetailTax.TaxLedger.LedgerName : null;

                return salesInvoiceDetailTaxModel;
            });
        }
    }
}
