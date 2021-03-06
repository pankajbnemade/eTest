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
    public class PurchaseInvoiceDetailService : Repository<Purchaseinvoicedetail>, IPurchaseInvoiceDetail
    {
        private readonly IPurchaseInvoice _purchaseInvoice;

        public PurchaseInvoiceDetailService(ErpDbContext dbContext, IPurchaseInvoice purchaseInvoice) : base(dbContext)
        {
            _purchaseInvoice = purchaseInvoice;
        }

        public async Task<int> GenerateSrNo(int purchaseInvoiceId)
        {
            int srNo = 0;

            if (await Any(w => w.PurchaseInvoiceDetId != 0 && w.PurchaseInvoiceId == purchaseInvoiceId))
            {
                srNo = await GetQueryByCondition(w => w.PurchaseInvoiceDetId != 0 && w.PurchaseInvoiceId == purchaseInvoiceId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel)
        {
            int purchaseInvoiceDetailId = 0;

            // assign values.
            Purchaseinvoicedetail purchaseInvoiceDetail = new Purchaseinvoicedetail();

            purchaseInvoiceDetail.PurchaseInvoiceId = purchaseInvoiceDetailModel.PurchaseInvoiceId;
            purchaseInvoiceDetail.SrNo = purchaseInvoiceDetailModel.SrNo;
            purchaseInvoiceDetail.Description = purchaseInvoiceDetailModel.Description;
            purchaseInvoiceDetail.UnitOfMeasurementId = purchaseInvoiceDetailModel.UnitOfMeasurementId;
            purchaseInvoiceDetail.Quantity = purchaseInvoiceDetailModel.Quantity;
            purchaseInvoiceDetail.PerUnit = purchaseInvoiceDetailModel.PerUnit;
            purchaseInvoiceDetail.UnitPrice = purchaseInvoiceDetailModel.UnitPrice;
            purchaseInvoiceDetail.GrossAmountFc = 0;
            purchaseInvoiceDetail.GrossAmount = 0;
            purchaseInvoiceDetail.TaxAmountFc = 0;
            purchaseInvoiceDetail.TaxAmount = 0;
            purchaseInvoiceDetail.NetAmountFc = 0;
            purchaseInvoiceDetail.NetAmount = 0;

            await Create(purchaseInvoiceDetail);

            purchaseInvoiceDetailId = purchaseInvoiceDetail.PurchaseInvoiceDetId;

            if (purchaseInvoiceDetailId != 0)
            {
                await UpdatePurchaseInvoiceDetailAmount(purchaseInvoiceDetailId);
            }

            return purchaseInvoiceDetailId; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicedetail purchaseInvoiceDetail = await GetByIdAsync(w => w.PurchaseInvoiceDetId == purchaseInvoiceDetailModel.PurchaseInvoiceDetId);

            if (null != purchaseInvoiceDetail)
            {
                // assign values.
                purchaseInvoiceDetail.PurchaseInvoiceId = purchaseInvoiceDetailModel.PurchaseInvoiceId;
                purchaseInvoiceDetail.SrNo = purchaseInvoiceDetailModel.SrNo;
                purchaseInvoiceDetail.Description = purchaseInvoiceDetailModel.Description;
                purchaseInvoiceDetail.UnitOfMeasurementId = purchaseInvoiceDetailModel.UnitOfMeasurementId;
                purchaseInvoiceDetail.Quantity = purchaseInvoiceDetailModel.Quantity;
                purchaseInvoiceDetail.PerUnit = purchaseInvoiceDetailModel.PerUnit;
                purchaseInvoiceDetail.UnitPrice = purchaseInvoiceDetailModel.UnitPrice;
                purchaseInvoiceDetail.GrossAmountFc = 0;
                purchaseInvoiceDetail.GrossAmount = 0;
                purchaseInvoiceDetail.TaxAmountFc = 0;
                purchaseInvoiceDetail.TaxAmount = 0;
                purchaseInvoiceDetail.NetAmountFc = 0;
                purchaseInvoiceDetail.NetAmount = 0;

                isUpdated = await Update(purchaseInvoiceDetail);
            }

            if (isUpdated != false)
            {
                await UpdatePurchaseInvoiceDetailAmount(purchaseInvoiceDetailModel.PurchaseInvoiceDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceDetailAmount(int? purchaseInvoiceDetailId)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicedetail purchaseInvoiceDetail = await GetQueryByCondition(w => w.PurchaseInvoiceDetId == purchaseInvoiceDetailId)
                                                                 .Include(w => w.PurchaseInvoice).Include(w => w.Purchaseinvoicedetailtaxes).FirstOrDefaultAsync();

            if (null != purchaseInvoiceDetail)
            {
                purchaseInvoiceDetail.GrossAmountFc = purchaseInvoiceDetail.Quantity * purchaseInvoiceDetail.PerUnit * purchaseInvoiceDetail.UnitPrice;
                purchaseInvoiceDetail.GrossAmount = purchaseInvoiceDetail.GrossAmountFc / purchaseInvoiceDetail.PurchaseInvoice.ExchangeRate;
                purchaseInvoiceDetail.TaxAmountFc = purchaseInvoiceDetail.Purchaseinvoicedetailtaxes.Sum(s => s.TaxAmountFc);
                purchaseInvoiceDetail.TaxAmount = purchaseInvoiceDetail.TaxAmountFc / purchaseInvoiceDetail.PurchaseInvoice.ExchangeRate;
                purchaseInvoiceDetail.NetAmountFc = purchaseInvoiceDetail.TaxAmountFc + purchaseInvoiceDetail.GrossAmountFc;
                purchaseInvoiceDetail.NetAmount = purchaseInvoiceDetail.NetAmountFc / purchaseInvoiceDetail.PurchaseInvoice.ExchangeRate;

                isUpdated = await Update(purchaseInvoiceDetail);
            }

            if (isUpdated != false)
            {
                await _purchaseInvoice.UpdatePurchaseInvoiceMasterAmount(purchaseInvoiceDetail.PurchaseInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeletePurchaseInvoiceDetail(int purchaseInvoiceDetailId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicedetail purchaseInvoiceDetail = await GetByIdAsync(w => w.PurchaseInvoiceDetId == purchaseInvoiceDetailId);

            if (null != purchaseInvoiceDetail)
            {
                isDeleted = await Delete(purchaseInvoiceDetail);
            }

            if (isDeleted != false)
            {
                await _purchaseInvoice.UpdatePurchaseInvoiceMasterAmount(purchaseInvoiceDetail.PurchaseInvoiceId);
            }

            return isDeleted; // returns.
        }

        public async Task<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetailById(int purchaseInvoiceDetailId)
        {
            PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = null;

            IList<PurchaseInvoiceDetailModel> purchaseInvoiceModelDetailList = await GetPurchaseInvoiceDetailList(purchaseInvoiceDetailId, 0);

            if (null != purchaseInvoiceModelDetailList && purchaseInvoiceModelDetailList.Any())
            {
                purchaseInvoiceDetailModel = purchaseInvoiceModelDetailList.FirstOrDefault();
            }

            return purchaseInvoiceDetailModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceDetailModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();

            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = await GetPurchaseInvoiceDetailList(0, purchaseInvoiceId);

            if (null != purchaseInvoiceDetailModelList && purchaseInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();
                resultModel.ResultList = purchaseInvoiceDetailModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();
                resultModel.ResultList = new List<PurchaseInvoiceDetailModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<IList<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailListByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = await GetPurchaseInvoiceDetailList(0, purchaseInvoiceId);

            return purchaseInvoiceDetailModelList; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailList()
        {
            DataTableResultModel<PurchaseInvoiceDetailModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();

            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = await GetPurchaseInvoiceDetailList(0, 0);

            if (null != purchaseInvoiceDetailModelList && purchaseInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();
                resultModel.ResultList = purchaseInvoiceDetailModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailList(int purchaseInvoiceDetailId, int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = null;

            // create query.
            IQueryable<Purchaseinvoicedetail> query = GetQueryByCondition(w => w.PurchaseInvoiceDetId != 0)
                                                        .Include(w => w.UnitOfMeasurement).Include(w => w.PurchaseInvoice);

            // apply filters.
            if (0 != purchaseInvoiceDetailId)
                query = query.Where(w => w.PurchaseInvoiceDetId == purchaseInvoiceDetailId);

            if (0 != purchaseInvoiceId)
                query = query.Where(w => w.PurchaseInvoiceId == purchaseInvoiceId);

            // get records by query.
            List<Purchaseinvoicedetail> purchaseInvoiceDetailList = await query.ToListAsync();

            if (null != purchaseInvoiceDetailList && purchaseInvoiceDetailList.Count > 0)
            {
                purchaseInvoiceDetailModelList = new List<PurchaseInvoiceDetailModel>();

                foreach (Purchaseinvoicedetail purchaseInvoiceDetail in purchaseInvoiceDetailList)
                {
                    purchaseInvoiceDetailModelList.Add(await AssignValueToModel(purchaseInvoiceDetail));
                }
            }

            return purchaseInvoiceDetailModelList; // returns.
        }

        private async Task<PurchaseInvoiceDetailModel> AssignValueToModel(Purchaseinvoicedetail purchaseInvoiceDetail)
        {
            return await Task.Run(() =>
            {
                PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = new PurchaseInvoiceDetailModel();

                purchaseInvoiceDetailModel.PurchaseInvoiceDetId = purchaseInvoiceDetail.PurchaseInvoiceDetId;
                purchaseInvoiceDetailModel.PurchaseInvoiceId = purchaseInvoiceDetail.PurchaseInvoiceId;
                purchaseInvoiceDetailModel.SrNo = purchaseInvoiceDetail.SrNo;
                purchaseInvoiceDetailModel.Description = purchaseInvoiceDetail.Description;
                purchaseInvoiceDetailModel.UnitOfMeasurementId = purchaseInvoiceDetail.UnitOfMeasurementId;
                purchaseInvoiceDetailModel.Quantity = purchaseInvoiceDetail.Quantity;
                purchaseInvoiceDetailModel.PerUnit = purchaseInvoiceDetail.PerUnit;
                purchaseInvoiceDetailModel.UnitPrice = purchaseInvoiceDetail.UnitPrice;
                purchaseInvoiceDetailModel.GrossAmountFc = purchaseInvoiceDetail.GrossAmountFc;
                purchaseInvoiceDetailModel.GrossAmount = purchaseInvoiceDetail.GrossAmount;
                purchaseInvoiceDetailModel.UnitPrice = purchaseInvoiceDetail.UnitPrice;
                purchaseInvoiceDetailModel.TaxAmountFc = purchaseInvoiceDetail.TaxAmountFc;
                purchaseInvoiceDetailModel.TaxAmount = purchaseInvoiceDetail.TaxAmount;
                purchaseInvoiceDetailModel.NetAmountFc = purchaseInvoiceDetail.NetAmountFc;
                purchaseInvoiceDetailModel.NetAmount = purchaseInvoiceDetail.NetAmount;

                //--####
                purchaseInvoiceDetailModel.UnitOfMeasurementName = null != purchaseInvoiceDetail.UnitOfMeasurement ? purchaseInvoiceDetail.UnitOfMeasurement.UnitOfMeasurementName : null;
                purchaseInvoiceDetailModel.IsTaxDetVisible = null != purchaseInvoiceDetail.PurchaseInvoice ?
                                                            (purchaseInvoiceDetail.PurchaseInvoice.TaxModelType == TaxModelType.LineWise.ToString() ? true : false)
                                                            : false;

                return purchaseInvoiceDetailModel;
            });
        }

    }
}
