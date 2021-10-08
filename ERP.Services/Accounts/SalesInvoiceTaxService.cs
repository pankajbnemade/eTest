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
    public class SalesInvoiceTaxService : Repository<Salesinvoicetax>, ISalesInvoiceTax
    {
        private readonly ISalesInvoice salesInvoice;
        private readonly ITaxRegisterDetail taxRegisterDetail;

        public SalesInvoiceTaxService(ErpDbContext dbContext, ISalesInvoice _salesInvoice, ITaxRegisterDetail _taxRegisterDetail) : base(dbContext)
        {
            salesInvoice = _salesInvoice;
            taxRegisterDetail = _taxRegisterDetail;
        }

        /// <summary>
        /// generate sr no based on salesInvoiceId
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int salesInvoiceId)
        {
            int srNo = 0;

            if (await Any(w => w.SalesInvoiceTaxId != 0 && w.SalesInvoiceId == salesInvoiceId))
            {
                srNo = await GetQueryByCondition(w => w.SalesInvoiceTaxId != 0 && w.SalesInvoiceId == salesInvoiceId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel)
        {
            int salesInvoiceTaxId = 0;

            // assign values.
            SalesInvoiceModel salesInvoiceModel = null;

            salesInvoiceModel = await salesInvoice.GetSalesInvoiceById((int)salesInvoiceTaxModel.SalesInvoiceId);

            Salesinvoicetax salesInvoiceTax = new Salesinvoicetax();

            salesInvoiceTax.SalesInvoiceId = salesInvoiceTaxModel.SalesInvoiceId;
            salesInvoiceTax.SrNo = salesInvoiceTaxModel.SrNo;
            salesInvoiceTax.TaxLedgerId = salesInvoiceTaxModel.TaxLedgerId;
            salesInvoiceTax.TaxPercentageOrAmount = salesInvoiceTaxModel.TaxPercentageOrAmount;
            salesInvoiceTax.TaxPerOrAmountFc = salesInvoiceTaxModel.TaxPerOrAmountFc == null ? 0 : salesInvoiceTaxModel.TaxPerOrAmountFc;
            salesInvoiceTax.TaxAddOrDeduct = salesInvoiceTaxModel.TaxAddOrDeduct;
            salesInvoiceTax.TaxAmountFc = 0;
            salesInvoiceTax.TaxAmount = 0;
            salesInvoiceTax.Remark = salesInvoiceTaxModel.Remark;

            await Create(salesInvoiceTax);
            salesInvoiceTaxId = salesInvoiceTax.SalesInvoiceTaxId;

            if (salesInvoiceTaxId != 0)
            {
                await UpdateSalesInvoiceTaxAmount(salesInvoiceTaxId); ;
            }

            return salesInvoiceTaxId; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceTax(SalesInvoiceTaxModel salesInvoiceTaxModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicetax salesInvoiceTax = await GetQueryByCondition(w => w.SalesInvoiceTaxId == salesInvoiceTaxModel.SalesInvoiceTaxId)
                        .Include(w => w.SalesInvoice).FirstOrDefaultAsync();

            if (null != salesInvoiceTax)
            {
                // assign values.

                salesInvoiceTax.SalesInvoiceId = salesInvoiceTaxModel.SalesInvoiceId;
                salesInvoiceTax.SrNo = salesInvoiceTaxModel.SrNo;
                salesInvoiceTax.TaxLedgerId = salesInvoiceTaxModel.TaxLedgerId;
                salesInvoiceTax.TaxPercentageOrAmount = salesInvoiceTaxModel.TaxPercentageOrAmount;
                salesInvoiceTax.TaxPerOrAmountFc = salesInvoiceTaxModel.TaxPerOrAmountFc == null ? 0 : salesInvoiceTaxModel.TaxPerOrAmountFc;
                salesInvoiceTax.TaxAddOrDeduct = salesInvoiceTaxModel.TaxAddOrDeduct;
                salesInvoiceTax.TaxAmountFc = 0;
                salesInvoiceTax.TaxAmount = 0;
                salesInvoiceTax.Remark = salesInvoiceTaxModel.Remark;

                isUpdated = await Update(salesInvoiceTax);
            }

            if (isUpdated != false)
            {
                await UpdateSalesInvoiceTaxAmount(salesInvoiceTaxModel.SalesInvoiceTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceTaxAmount(int? salesInvoiceTaxId)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Salesinvoicetax salesInvoiceTax = await GetQueryByCondition(w => w.SalesInvoiceTaxId == salesInvoiceTaxId)
                                                                 .Include(w => w.SalesInvoice).FirstOrDefaultAsync();

            if (null != salesInvoice)
            {
                if (DiscountType.Percentage.ToString() == salesInvoiceTax.TaxPercentageOrAmount)
                {
                    salesInvoiceTax.TaxAmountFc = (salesInvoiceTax.SalesInvoice.GrossAmountFc * salesInvoiceTax.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    salesInvoiceTax.TaxAmountFc = salesInvoiceTax.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == salesInvoiceTax.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                salesInvoiceTax.TaxAmountFc = multiplier * salesInvoiceTax.TaxAmountFc;

                salesInvoiceTax.TaxAmount = salesInvoiceTax.TaxAmountFc / salesInvoiceTax.SalesInvoice.ExchangeRate;

                isUpdated = await Update(salesInvoiceTax);
            }

            if (isUpdated != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceTax.SalesInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceTaxAmountAll(int? salesInvoiceId)
        {
            bool isUpdated = false;

            // get record.
            IList<Salesinvoicetax> salesInvoiceTaxList = await GetQueryByCondition(w => w.SalesInvoiceId == (int)salesInvoiceId).ToListAsync();

            foreach (Salesinvoicetax salesInvoiceTax in salesInvoiceTaxList)
            {
                isUpdated = await UpdateSalesInvoiceTaxAmount(salesInvoiceTax.SalesInvoiceTaxId);
            }

            if (isUpdated != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceTax(int salesInvoiceTaxId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicetax salesInvoiceTax = await GetByIdAsync(w => w.SalesInvoiceTaxId == salesInvoiceTaxId);

            if (null != salesInvoiceTax)
            {
                isDeleted = await Delete(salesInvoiceTax);
            }

            if (isDeleted != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceTax.SalesInvoiceId);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> AddSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId, int taxRegisterId)
        {
            bool isUpdated = false;

            // get record.
            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            SalesInvoiceTaxModel salesInvoiceTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    salesInvoiceTaxModel = new SalesInvoiceTaxModel()
                    {
                        SalesInvoiceTaxId = 0,
                        SalesInvoiceId = salesInvoiceId,
                        SrNo = (int)taxRegisterDetailModel.SrNo,
                        TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                        TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                        TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                        TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                        TaxAmountFc = 0,
                        TaxAmount = 0,
                        Remark = ""
                    };

                    await CreateSalesInvoiceTax(salesInvoiceTaxModel);
                }
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceTaxBySalesInvoiceId(int salesInvoiceId)
        {
            bool isDeleted = false;

            // get record.
            IList<Salesinvoicetax> salesInvoiceTaxList = await GetQueryByCondition(w => w.SalesInvoiceId == (int)salesInvoiceId).ToListAsync();

            foreach (Salesinvoicetax salesInvoiceTax in salesInvoiceTaxList)
            {
                isDeleted = await DeleteSalesInvoiceTax(salesInvoiceTax.SalesInvoiceTaxId);
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
            else
            {
                resultModel = new DataTableResultModel<SalesInvoiceTaxModel>();
                resultModel.ResultList = new List<SalesInvoiceTaxModel>();
                resultModel.TotalResultCount = 0;
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
            IQueryable<Salesinvoicetax> query = GetQueryByCondition(w => w.SalesInvoiceTaxId != 0).Include(t => t.TaxLedger);

            // apply filters.
            if (0 != salesInvoiceTaxId)
                query = query.Where(w => w.SalesInvoiceTaxId == salesInvoiceTaxId);

            // apply filters.
            if (0 != salesInvoiceId)
                query = query.Where(w => w.SalesInvoiceId == salesInvoiceId);

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

                salesInvoiceTaxModel.SalesInvoiceTaxId = salesInvoiceTax.SalesInvoiceTaxId;
                salesInvoiceTaxModel.SalesInvoiceId = salesInvoiceTax.SalesInvoiceId;
                salesInvoiceTaxModel.SrNo = salesInvoiceTax.SrNo;
                salesInvoiceTaxModel.TaxLedgerId = salesInvoiceTax.TaxLedgerId;
                salesInvoiceTaxModel.TaxPercentageOrAmount = salesInvoiceTax.TaxPercentageOrAmount;
                salesInvoiceTaxModel.TaxPerOrAmountFc = salesInvoiceTax.TaxPerOrAmountFc;
                salesInvoiceTaxModel.TaxAddOrDeduct = salesInvoiceTax.TaxAddOrDeduct;
                salesInvoiceTaxModel.TaxAmountFc = salesInvoiceTax.TaxAmountFc;
                salesInvoiceTaxModel.TaxAmount = salesInvoiceTax.TaxAmount;
                salesInvoiceTaxModel.Remark = salesInvoiceTax.Remark;

                salesInvoiceTaxModel.TaxLedgerName = null != salesInvoiceTax.TaxLedger ? salesInvoiceTax.TaxLedger.LedgerName : null;

                return salesInvoiceTaxModel;
            });
        }
    }
}
