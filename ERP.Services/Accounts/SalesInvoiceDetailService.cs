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
    public class SalesInvoiceDetailService : Repository<Salesinvoicedetail>, ISalesInvoiceDetail
    {
        public SalesInvoiceDetailService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            int salesInvoiceDetailId = 0;

            // assign values.
            Salesinvoicedetail salesInvoiceDetail = new Salesinvoicedetail();
            
            salesInvoiceDetail.InvoiceId = salesInvoiceDetailModel.InvoiceId;
            salesInvoiceDetail.SrNo = salesInvoiceDetailModel.SrNo;
            salesInvoiceDetail.Description = salesInvoiceDetailModel.Description;
            salesInvoiceDetail.UnitOfMeasurementId = salesInvoiceDetailModel.UnitOfMeasurementId;
            salesInvoiceDetail.Quantity = salesInvoiceDetailModel.Quantity;
            salesInvoiceDetail.PerUnit = salesInvoiceDetailModel.PerUnit;
            salesInvoiceDetail.UnitPrice = salesInvoiceDetailModel.UnitPrice;
            salesInvoiceDetail.GrossAmountFc = salesInvoiceDetailModel.GrossAmountFc;
            salesInvoiceDetail.GrossAmount = salesInvoiceDetailModel.GrossAmount;
            salesInvoiceDetail.TaxAmountFc = 0;
            salesInvoiceDetail.TaxAmount = 0;
            salesInvoiceDetail.NetAmountFc = 0;
            salesInvoiceDetail.NetAmount = 0;

            salesInvoiceDetailId = await Create(salesInvoiceDetail);

            return salesInvoiceDetailId; // returns.
        }

        public async Task<bool> UpdateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetByIdAsync(w => w.InvoiceDetId == salesInvoiceDetailModel.InvoiceDetId);

            if (null != salesInvoiceDetail)
            {
                // assign values.
                salesInvoiceDetail.InvoiceId = salesInvoiceDetailModel.InvoiceId;
                salesInvoiceDetail.SrNo = salesInvoiceDetailModel.SrNo;
                salesInvoiceDetail.Description = salesInvoiceDetailModel.Description;
                salesInvoiceDetail.UnitOfMeasurementId = salesInvoiceDetailModel.UnitOfMeasurementId;
                salesInvoiceDetail.Quantity = salesInvoiceDetailModel.Quantity;
                salesInvoiceDetail.PerUnit = salesInvoiceDetailModel.PerUnit;
                salesInvoiceDetail.UnitPrice = salesInvoiceDetailModel.UnitPrice;
                salesInvoiceDetail.GrossAmountFc = salesInvoiceDetailModel.GrossAmountFc;
                salesInvoiceDetail.GrossAmount = salesInvoiceDetailModel.GrossAmount;
                salesInvoiceDetail.TaxAmountFc = 0;
                salesInvoiceDetail.TaxAmount = 0;
                salesInvoiceDetail.NetAmountFc = 0;
                salesInvoiceDetail.NetAmount = 0;

                isUpdated = await Update(salesInvoiceDetail);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeleteSalesInvoiceDetail(int salesInvoiceDetailId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetByIdAsync(w => w.InvoiceDetId == salesInvoiceDetailId);

            if (null != salesInvoiceDetail)
            {
                isDeleted = await Delete(salesInvoiceDetail);
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

        public async Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailBySalesInvoiceId(int SalesInvoiceId)
        {
            DataTableResultModel<SalesInvoiceDetailModel> resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();

            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await GetSalesInvoiceDetailList(0, SalesInvoiceId);

            if (null != salesInvoiceDetailModelList && salesInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();
                resultModel.ResultList = salesInvoiceDetailModelList;
                resultModel.TotalResultCount = salesInvoiceDetailModelList.Count();
            }

            return resultModel; // returns.
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
            IQueryable<Salesinvoicedetail> query = GetQueryByCondition(w => w.InvoiceDetId != 0)
                                                        .Include(w => w.UnitOfMeasurement);

            // apply filters.
            if (0 != salesInvoiceDetailId)
                query = query.Where(w => w.InvoiceDetId == salesInvoiceDetailId);

            if (0 != salesInvoiceId)
                query = query.Where(w => w.InvoiceId == salesInvoiceId);

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

                salesInvoiceDetailModel.InvoiceId = salesInvoiceDetail.InvoiceId;
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
                salesInvoiceDetailModel.UnitOfMeasurementName = salesInvoiceDetail.UnitOfMeasurement.UnitOfMeasurementName;

                return salesInvoiceDetailModel;
            });
        }

    }
}
