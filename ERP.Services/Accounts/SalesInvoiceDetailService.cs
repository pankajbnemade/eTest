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
    public class SalesInvoiceDetailService : Repository<Salesinvoicedetail>, ISalesInvoiceDetail
    {
        ISalesInvoice salesInvoice;

        public SalesInvoiceDetailService(ErpDbContext dbContext, ISalesInvoice _salesInvoice) : base(dbContext)
        {
            salesInvoice = _salesInvoice;
        }

        public async Task<int> GenerateSrNo(int salesInvoiceId)
        {
            int srNo = 0;

            if (await Any(w => w.SalesInvoiceDetId != 0 && w.SalesInvoiceId == salesInvoiceId))
            {
                srNo = await GetQueryByCondition(w => w.SalesInvoiceDetId != 0 && w.SalesInvoiceId == salesInvoiceId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            int salesInvoiceDetailId = 0;

            // assign values.
            Salesinvoicedetail salesInvoiceDetail = new Salesinvoicedetail();

            salesInvoiceDetail.SalesInvoiceId = salesInvoiceDetailModel.SalesInvoiceId;
            salesInvoiceDetail.SrNo = salesInvoiceDetailModel.SrNo;
            salesInvoiceDetail.Description = salesInvoiceDetailModel.Description;
            salesInvoiceDetail.UnitOfMeasurementId = salesInvoiceDetailModel.UnitOfMeasurementId;
            salesInvoiceDetail.Quantity = salesInvoiceDetailModel.Quantity == null ? 0 : salesInvoiceDetailModel.Quantity;
            salesInvoiceDetail.PerUnit = salesInvoiceDetailModel.PerUnit == null ? 0 : salesInvoiceDetailModel.PerUnit;
            salesInvoiceDetail.UnitPrice = salesInvoiceDetailModel.UnitPrice == null ? 0 : salesInvoiceDetailModel.UnitPrice;
            salesInvoiceDetail.GrossAmountFc = 0;
            salesInvoiceDetail.GrossAmount = 0;
            salesInvoiceDetail.TaxAmountFc = 0;
            salesInvoiceDetail.TaxAmount = 0;
            salesInvoiceDetail.NetAmountFc = 0;
            salesInvoiceDetail.NetAmount = 0;

            await Create(salesInvoiceDetail);

            salesInvoiceDetailId = salesInvoiceDetail.SalesInvoiceDetId;

            if (salesInvoiceDetailId != 0)
            {
                await UpdateSalesInvoiceDetailAmount(salesInvoiceDetailId);
            }

            return salesInvoiceDetailId; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetByIdAsync(w => w.SalesInvoiceDetId == salesInvoiceDetailModel.SalesInvoiceDetId);

            if (null != salesInvoiceDetail)
            {

                // assign values.
                salesInvoiceDetail.SalesInvoiceId = salesInvoiceDetailModel.SalesInvoiceId;
                salesInvoiceDetail.SrNo = salesInvoiceDetailModel.SrNo;
                salesInvoiceDetail.Description = salesInvoiceDetailModel.Description;
                salesInvoiceDetail.UnitOfMeasurementId = salesInvoiceDetailModel.UnitOfMeasurementId;
                salesInvoiceDetail.Quantity = salesInvoiceDetailModel.Quantity == null ? 0 : salesInvoiceDetailModel.Quantity;
                salesInvoiceDetail.PerUnit = salesInvoiceDetailModel.PerUnit == null ? 0 : salesInvoiceDetailModel.PerUnit;
                salesInvoiceDetail.UnitPrice = salesInvoiceDetailModel.UnitPrice == null ? 0 : salesInvoiceDetailModel.UnitPrice;
                salesInvoiceDetail.GrossAmountFc = 0;
                salesInvoiceDetail.GrossAmount = 0;
                salesInvoiceDetail.TaxAmountFc = 0;
                salesInvoiceDetail.TaxAmount = 0;
                salesInvoiceDetail.NetAmountFc = 0;
                salesInvoiceDetail.NetAmount = 0;

                isUpdated = await Update(salesInvoiceDetail);
            }

            if (isUpdated != false)
            {
                await UpdateSalesInvoiceDetailAmount(salesInvoiceDetailModel.SalesInvoiceDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetailAmount(int? salesInvoiceDetailId)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetQueryByCondition(w => w.SalesInvoiceDetId == salesInvoiceDetailId)
                                                                 .Include(w => w.SalesInvoice).Include(w => w.Salesinvoicedetailtaxes).FirstOrDefaultAsync();

            if (null != salesInvoiceDetail)
            {
                salesInvoiceDetail.GrossAmountFc = salesInvoiceDetail.Quantity * salesInvoiceDetail.PerUnit * salesInvoiceDetail.UnitPrice;
                salesInvoiceDetail.GrossAmount = salesInvoiceDetail.GrossAmountFc / salesInvoiceDetail.SalesInvoice.ExchangeRate;
                salesInvoiceDetail.TaxAmountFc = salesInvoiceDetail.Salesinvoicedetailtaxes.Sum(s => s.TaxAmountFc);
                salesInvoiceDetail.TaxAmount = salesInvoiceDetail.TaxAmountFc / salesInvoiceDetail.SalesInvoice.ExchangeRate;
                salesInvoiceDetail.NetAmountFc = salesInvoiceDetail.TaxAmountFc + salesInvoiceDetail.GrossAmountFc;
                salesInvoiceDetail.NetAmount = salesInvoiceDetail.NetAmountFc / salesInvoiceDetail.SalesInvoice.ExchangeRate;

                isUpdated = await Update(salesInvoiceDetail);
            }

            if (isUpdated != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceDetail.SalesInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceDetail(int salesInvoiceDetailId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetByIdAsync(w => w.SalesInvoiceDetId == salesInvoiceDetailId);

            if (null != salesInvoiceDetail)
            {
                isDeleted = await Delete(salesInvoiceDetail);
            }

            if (isDeleted != false)
            {
                await salesInvoice.UpdateSalesInvoiceMasterAmount(salesInvoiceDetail.SalesInvoiceId);
            }

            return isDeleted; // returns.
        }

        public async Task<SalesInvoiceDetailModel> GetSalesInvoiceDetailById(int salesInvoiceDetailId)
        {
            SalesInvoiceDetailModel salesInvoiceDetailModel = null;

            IList<SalesInvoiceDetailModel> salesInvoiceModelDetailList = await GetSalesInvoiceDetailList(salesInvoiceDetailId, 0);

            if (null != salesInvoiceModelDetailList && salesInvoiceModelDetailList.Any())
            {
                salesInvoiceDetailModel = salesInvoiceModelDetailList.FirstOrDefault();
            }

            return salesInvoiceDetailModel; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailBySalesInvoiceId(int salesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceDetailModel> resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();

            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await GetSalesInvoiceDetailList(0, salesInvoiceId);

            if (null != salesInvoiceDetailModelList && salesInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();
                resultModel.ResultList = salesInvoiceDetailModelList;
                resultModel.TotalResultCount = salesInvoiceDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();
                resultModel.ResultList = new List<SalesInvoiceDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<IList<SalesInvoiceDetailModel>> GetSalesInvoiceDetailListBySalesInvoiceId(int salesInvoiceId)
        {
            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await GetSalesInvoiceDetailList(0, salesInvoiceId);

            return salesInvoiceDetailModelList; // returns.
        }

        public async Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList()
        {
            DataTableResultModel<SalesInvoiceDetailModel> resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();

            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await GetSalesInvoiceDetailList(0, 0);

            if (null != salesInvoiceDetailModelList && salesInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();
                resultModel.ResultList = salesInvoiceDetailModelList;
                resultModel.TotalResultCount = salesInvoiceDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList(int salesInvoiceDetailId, int salesInvoiceId)
        {
            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = null;

            // create query.
            IQueryable<Salesinvoicedetail> query = GetQueryByCondition(w => w.SalesInvoiceDetId != 0)
                                                        .Include(w => w.UnitOfMeasurement).Include(w => w.SalesInvoice);

            // apply filters.
            if (0 != salesInvoiceDetailId)
                query = query.Where(w => w.SalesInvoiceDetId == salesInvoiceDetailId);

            if (0 != salesInvoiceId)
                query = query.Where(w => w.SalesInvoiceId == salesInvoiceId);

            // get records by query.
            List<Salesinvoicedetail> salesInvoiceDetailList = await query.ToListAsync();

            if (null != salesInvoiceDetailList && salesInvoiceDetailList.Count > 0)
            {
                salesInvoiceDetailModelList = new List<SalesInvoiceDetailModel>();

                foreach (Salesinvoicedetail salesInvoiceDetail in salesInvoiceDetailList)
                {
                    salesInvoiceDetailModelList.Add(await AssignValueToModel(salesInvoiceDetail));
                }
            }

            return salesInvoiceDetailModelList; // returns.
        }

        private async Task<SalesInvoiceDetailModel> AssignValueToModel(Salesinvoicedetail salesInvoiceDetail)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceDetailModel salesInvoiceDetailModel = new SalesInvoiceDetailModel();

                salesInvoiceDetailModel.SalesInvoiceDetId = salesInvoiceDetail.SalesInvoiceDetId;
                salesInvoiceDetailModel.SalesInvoiceId = salesInvoiceDetail.SalesInvoiceId;
                salesInvoiceDetailModel.SrNo = salesInvoiceDetail.SrNo;
                salesInvoiceDetailModel.Description = salesInvoiceDetail.Description;
                salesInvoiceDetailModel.UnitOfMeasurementId = salesInvoiceDetail.UnitOfMeasurementId;
                salesInvoiceDetailModel.Quantity = salesInvoiceDetail.Quantity;
                salesInvoiceDetailModel.PerUnit = salesInvoiceDetail.PerUnit;
                salesInvoiceDetailModel.UnitPrice = salesInvoiceDetail.UnitPrice;
                salesInvoiceDetailModel.GrossAmountFc = salesInvoiceDetail.GrossAmountFc;
                salesInvoiceDetailModel.GrossAmount = salesInvoiceDetail.GrossAmount;
                salesInvoiceDetailModel.UnitPrice = salesInvoiceDetail.UnitPrice;
                salesInvoiceDetailModel.TaxAmountFc = salesInvoiceDetail.TaxAmountFc;
                salesInvoiceDetailModel.TaxAmount = salesInvoiceDetail.TaxAmount;
                salesInvoiceDetailModel.NetAmountFc = salesInvoiceDetail.NetAmountFc;
                salesInvoiceDetailModel.NetAmount = salesInvoiceDetail.NetAmount;

                //--####
                salesInvoiceDetailModel.UnitOfMeasurementName = null != salesInvoiceDetail.UnitOfMeasurement ? salesInvoiceDetail.UnitOfMeasurement.UnitOfMeasurementName : null;
                salesInvoiceDetailModel.IsTaxDetVisible = null != salesInvoiceDetail.SalesInvoice ?
                                                            (salesInvoiceDetail.SalesInvoice.TaxModelType == TaxModelType.LineWise.ToString() ? true : false)
                                                            : false;

                return salesInvoiceDetailModel;
            });
        }

    }
}
