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
        private readonly ISalesInvoiceDetail _salesInvoiceDetail;
        private readonly ITaxRegisterDetail _taxRegisterDetail;


        public SalesInvoiceDetailTaxService(ErpDbContext dbContext, ISalesInvoiceDetail salesInvoiceDetail, ITaxRegisterDetail taxRegisterDetail) : base(dbContext)
        {
            _salesInvoiceDetail = salesInvoiceDetail;
            _taxRegisterDetail = taxRegisterDetail;
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

            //SalesInvoiceDetailModel salesInvoiceDetailModel = null;
            //salesInvoiceDetailModel = await salesInvoiceDetail.GetSalesInvoiceDetailById((Int32)salesInvoiceDetailTaxModel.SalesInvoiceDetId);

            // assign values.
            Salesinvoicedetailtax salesInvoiceDetailTax = new Salesinvoicedetailtax();

            salesInvoiceDetailTax.SalesInvoiceDetId = salesInvoiceDetailTaxModel.SalesInvoiceDetId;
            salesInvoiceDetailTax.SrNo = salesInvoiceDetailTaxModel.SrNo;
            salesInvoiceDetailTax.TaxLedgerId = salesInvoiceDetailTaxModel.TaxLedgerId;
            salesInvoiceDetailTax.TaxPercentageOrAmount = salesInvoiceDetailTaxModel.TaxPercentageOrAmount;
            salesInvoiceDetailTax.TaxPerOrAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;
            salesInvoiceDetailTax.TaxAddOrDeduct = salesInvoiceDetailTaxModel.TaxAddOrDeduct;
            salesInvoiceDetailTax.TaxAmountFc = 0;
            salesInvoiceDetailTax.TaxAmount = 0;
            salesInvoiceDetailTax.Remark = salesInvoiceDetailTaxModel.Remark;

            await Create(salesInvoiceDetailTax);

            salesInvoiceDetailTaxId = salesInvoiceDetailTax.SalesInvoiceDetTaxId;

            if (salesInvoiceDetailTaxId != 0)
            {
                await UpdateSalesInvoiceDetailTaxAmount(salesInvoiceDetailTaxId);
            }

            return salesInvoiceDetailTaxId; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetailTax(SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel)
        {
            bool isUpdated = false;

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
                salesInvoiceDetailTax.TaxPerOrAmountFc = salesInvoiceDetailTaxModel.TaxPerOrAmountFc;;
                salesInvoiceDetailTax.TaxAddOrDeduct = salesInvoiceDetailTaxModel.TaxAddOrDeduct;
                salesInvoiceDetailTax.TaxAmountFc = 0;
                salesInvoiceDetailTax.TaxAmount = 0;
                salesInvoiceDetailTax.Remark = salesInvoiceDetailTaxModel.Remark;

                isUpdated = await Update(salesInvoiceDetailTax);
            }

            if (isUpdated != false)
            {
                await UpdateSalesInvoiceDetailTaxAmount(salesInvoiceDetailTaxModel.SalesInvoiceDetTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetailTaxAmount(int? salesInvoiceDetailTaxId)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Salesinvoicedetailtax salesInvoiceDetailTax = await GetQueryByCondition(w => w.SalesInvoiceDetTaxId == salesInvoiceDetailTaxId)
                                                                .Include(w => w.SalesInvoiceDet).ThenInclude(w => w.SalesInvoice).FirstOrDefaultAsync();

            if (null != _salesInvoiceDetail)
            {
                if (DiscountType.Percentage.ToString() == salesInvoiceDetailTax.TaxPercentageOrAmount)
                {
                    salesInvoiceDetailTax.TaxAmountFc = (salesInvoiceDetailTax.SalesInvoiceDet.GrossAmountFc * salesInvoiceDetailTax.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    salesInvoiceDetailTax.TaxAmountFc = salesInvoiceDetailTax.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == salesInvoiceDetailTax.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                salesInvoiceDetailTax.TaxAmountFc = multiplier * salesInvoiceDetailTax.TaxAmountFc;

                salesInvoiceDetailTax.TaxAmount = salesInvoiceDetailTax.TaxAmountFc / salesInvoiceDetailTax.SalesInvoiceDet.SalesInvoice.ExchangeRate;

                isUpdated = await Update(salesInvoiceDetailTax);
            }

            if (isUpdated != false)
            {
                await _salesInvoiceDetail.UpdateSalesInvoiceDetailAmount(salesInvoiceDetailTax.SalesInvoiceDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetailTaxAmountOnDetailUpdate(int? salesInvoiceDetailId)
        {
            bool isUpdated = false;

            // get record.
            IList<Salesinvoicedetailtax> salesInvoiceDetailTaxList = await GetQueryByCondition(w => w.SalesInvoiceDetId == (int)salesInvoiceDetailId).ToListAsync();

            foreach (Salesinvoicedetailtax salesInvoiceDetailTax in salesInvoiceDetailTaxList)
            {
                isUpdated = await UpdateSalesInvoiceDetailTaxAmount(salesInvoiceDetailTax.SalesInvoiceDetTaxId);
            }

            if (isUpdated != false)
            {
                await _salesInvoiceDetail.UpdateSalesInvoiceDetailAmount(salesInvoiceDetailId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceDetailTax(int salesInvoiceDetailTaxId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicedetailtax salesInvoiceDetailTax = await GetByIdAsync(w => w.SalesInvoiceDetTaxId == salesInvoiceDetailTaxId);

            if (null != salesInvoiceDetailTax)
            {
                isDeleted = await Delete(salesInvoiceDetailTax);
            }

            if (isDeleted != false)
            {
                await _salesInvoiceDetail.UpdateSalesInvoiceDetailAmount(salesInvoiceDetailTax.SalesInvoiceDetId);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> AddSalesInvoiceDetailTaxBySalesInvoiceId(int salesInvoiceId, int taxRegisterId)
        {
            bool isUpdated = false;

            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await _salesInvoiceDetail.GetSalesInvoiceDetailListBySalesInvoiceId(salesInvoiceId);

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await _taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    foreach (SalesInvoiceDetailModel salesInvoiceDetailModel in salesInvoiceDetailModelList)
                    {
                        salesInvoiceDetailTaxModel = new SalesInvoiceDetailTaxModel()
                        {
                            SalesInvoiceDetTaxId = 0,
                            SalesInvoiceDetId = salesInvoiceDetailModel.SalesInvoiceDetId,
                             SrNo = (int)taxRegisterDetailModel.SrNo,
                            TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                            TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                            TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                            TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                            TaxAmountFc = 0,
                            TaxAmount = 0,
                            Remark = ""
                        };

                        await CreateSalesInvoiceDetailTax(salesInvoiceDetailTaxModel);
                    }
                }
            }

            return isUpdated; // returns.
        }

        public async Task<bool> AddSalesInvoiceDetailTaxBySalesInvoiceDetId(int salesInvoiceDetId, int taxRegisterId)
        {
            bool isUpdated = false;

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await _taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            SalesInvoiceDetailTaxModel salesInvoiceDetailTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    salesInvoiceDetailTaxModel = new SalesInvoiceDetailTaxModel()
                    {
                        SalesInvoiceDetTaxId = 0,
                        SalesInvoiceDetId = salesInvoiceDetId,
                         SrNo = (int)taxRegisterDetailModel.SrNo,
                            TaxLedgerId = (int)taxRegisterDetailModel.TaxLedgerId,
                            TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                            TaxPerOrAmountFc = (decimal)taxRegisterDetailModel.Rate,
                            TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                        TaxAmountFc = 0,
                        TaxAmount = 0,
                        Remark = ""
                    };

                    await CreateSalesInvoiceDetailTax(salesInvoiceDetailTaxModel);
                }
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteSalesInvoiceDetailTaxBySalesInvoiceId(int salesInvoiceId)
        {
            bool isDeleted = false;

            // get record.
            IList<Salesinvoicedetailtax> salesInvoiceDetailTaxList = await GetQueryByCondition(w => w.SalesInvoiceDetTaxId != 0).Include(W => W.SalesInvoiceDet)
                                                                                .Where(W => W.SalesInvoiceDet.SalesInvoiceId == salesInvoiceId)
                                                                                .ToListAsync();

            foreach (Salesinvoicedetailtax salesInvoiceDetailTax in salesInvoiceDetailTaxList)
            {
                isDeleted = await DeleteSalesInvoiceDetailTax(salesInvoiceDetailTax.SalesInvoiceDetTaxId);
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
            IQueryable<Salesinvoicedetailtax> query = GetQueryByCondition(w => w.SalesInvoiceDetTaxId != 0)
                                                        .Include(w => w.TaxLedger);

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

                salesInvoiceDetailTaxModel.TaxLedgerName = null != salesInvoiceDetailTax.TaxLedger ? salesInvoiceDetailTax.TaxLedger.LedgerName : null; ;

                return salesInvoiceDetailTaxModel;
            });
        }
    }
}
