﻿using ERP.DataAccess.EntityData;
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

            salesInvoiceDetailTax.InvoiceDetId = salesInvoiceDetailTaxModel.InvoiceDetId;
            salesInvoiceDetailTax.SrNo = salesInvoiceDetailTaxModel.SrNo;
            salesInvoiceDetailTax.TaxLedgerId = salesInvoiceDetailTaxModel.TaxLedgerId;
            salesInvoiceDetailTax.TaxPercentageOrAmount = salesInvoiceDetailTaxModel.TaxPercentageOrAmount;
            salesInvoiceDetailTax.TaxPerOrAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;
            salesInvoiceDetailTax.TaxAddOrDeduct = salesInvoiceDetailTaxModel.TaxAddOrDeduct;
            salesInvoiceDetailTax.TaxAmountFc = salesInvoiceDetailTaxModel.TaxAmountFc;
            salesInvoiceDetailTax.TaxAmount = salesInvoiceDetailTaxModel.TaxAmount;
            salesInvoiceDetailTax.Remark = salesInvoiceDetailTaxModel.Remark;

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
                salesInvoiceDetailTax.InvoiceDetId = salesInvoiceDetailTaxModel.InvoiceDetId;
                salesInvoiceDetailTax.SrNo = salesInvoiceDetailTaxModel.SrNo;
                salesInvoiceDetailTax.TaxLedgerId = salesInvoiceDetailTaxModel.TaxLedgerId;
                salesInvoiceDetailTax.TaxPercentageOrAmount = salesInvoiceDetailTaxModel.TaxPercentageOrAmount;
                salesInvoiceDetailTax.TaxPerOrAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;
                salesInvoiceDetailTax.TaxAddOrDeduct = salesInvoiceDetailTaxModel.TaxAddOrDeduct;
                salesInvoiceDetailTax.TaxAmountFc = salesInvoiceDetailTaxModel.TaxAmountFc;
                salesInvoiceDetailTax.TaxAmount = salesInvoiceDetailTaxModel.TaxAmount;
                salesInvoiceDetailTax.Remark = salesInvoiceDetailTaxModel.Remark;

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
            IQueryable<Salesinvoicedetailtax> query = GetQueryByCondition(w => w.InvoiceDetTaxId != 0)
                                                        .Include(w => w.TaxLedger);

            // apply filters.
            if (0 != salesInvoiceDetailTaxId)
                query = query.Where(w => w.InvoiceDetTaxId == salesInvoiceDetailTaxId);

            // apply filters.
            if (0 != salesInvoiceDetailId)
                query = query.Where(w => w.InvoiceDetId == salesInvoiceDetailId);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.InvoiceDet.InvoiceId == salesInvoiceId);

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

                salesInvoiceDetailTaxModel.InvoiceDetTaxId = salesInvoiceDetailTax.InvoiceDetTaxId;
                salesInvoiceDetailTaxModel.InvoiceDetId = salesInvoiceDetailTax.InvoiceDetId;
                salesInvoiceDetailTaxModel.SrNo = salesInvoiceDetailTax.SrNo;
                salesInvoiceDetailTaxModel.TaxLedgerId = salesInvoiceDetailTax.TaxLedgerId;
                salesInvoiceDetailTaxModel.TaxPercentageOrAmount = salesInvoiceDetailTax.TaxPercentageOrAmount;
                salesInvoiceDetailTaxModel.TaxPerOrAmountFc = salesInvoiceDetailTax.TaxPerOrAmountFc;
                salesInvoiceDetailTaxModel.TaxAddOrDeduct = salesInvoiceDetailTax.TaxAddOrDeduct;
                salesInvoiceDetailTaxModel.TaxAmountFc = salesInvoiceDetailTax.TaxAmountFc;
                salesInvoiceDetailTaxModel.TaxAmount = salesInvoiceDetailTax.TaxAmount;
                salesInvoiceDetailTaxModel.Remark = salesInvoiceDetailTax.Remark;

                salesInvoiceDetailTaxModel.TaxLedgerName = salesInvoiceDetailTax.TaxLedger.LedgerName;

                return salesInvoiceDetailTaxModel;
            });
        }
    }
}
