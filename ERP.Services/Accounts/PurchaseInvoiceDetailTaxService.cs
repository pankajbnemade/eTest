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
    public class PurchaseInvoiceDetailTaxService : Repository<Purchaseinvoicedetailtax>, IPurchaseInvoiceDetailTax
    {
        public PurchaseInvoiceDetailTaxService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel)
        {
            int purchaseInvoiceDetailTaxId = 0;

            // assign values.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = new Purchaseinvoicedetailtax();

            purchaseInvoiceDetailTax.InvoiceDetId = purchaseInvoiceDetailTaxModel.InvoiceDetId;
            purchaseInvoiceDetailTax.SrNo = purchaseInvoiceDetailTaxModel.SrNo;
            purchaseInvoiceDetailTax.TaxLedgerId = purchaseInvoiceDetailTaxModel.TaxLedgerId;
            purchaseInvoiceDetailTax.TaxPercentageOrAmount = purchaseInvoiceDetailTaxModel.TaxPercentageOrAmount;
            purchaseInvoiceDetailTax.TaxPerOrAmountFc = purchaseInvoiceDetailTaxModel.TaxPerOrAmountFc;
            purchaseInvoiceDetailTax.TaxAddOrDeduct = purchaseInvoiceDetailTaxModel.TaxAddOrDeduct;
            purchaseInvoiceDetailTax.TaxAmountFc = purchaseInvoiceDetailTaxModel.TaxAmountFc;
            purchaseInvoiceDetailTax.TaxAmount = purchaseInvoiceDetailTaxModel.TaxAmount;
            purchaseInvoiceDetailTax.Remark = purchaseInvoiceDetailTaxModel.Remark;

            purchaseInvoiceDetailTaxId = await Create(purchaseInvoiceDetailTax);

            return purchaseInvoiceDetailTaxId; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = await GetByIdAsync(w => w.InvoiceDetTaxId == purchaseInvoiceDetailTaxModel.InvoiceDetTaxId);

            if (null != purchaseInvoiceDetailTax)
            {
                // assign values.

                purchaseInvoiceDetailTax.SrNo = purchaseInvoiceDetailTaxModel.SrNo;
                purchaseInvoiceDetailTax.TaxLedgerId = purchaseInvoiceDetailTaxModel.TaxLedgerId;
                purchaseInvoiceDetailTax.TaxPercentageOrAmount = purchaseInvoiceDetailTaxModel.TaxPercentageOrAmount;
                purchaseInvoiceDetailTax.TaxPerOrAmountFc = purchaseInvoiceDetailTaxModel.TaxPerOrAmountFc;
                purchaseInvoiceDetailTax.TaxAddOrDeduct = purchaseInvoiceDetailTaxModel.TaxAddOrDeduct;
                purchaseInvoiceDetailTax.TaxAmountFc = purchaseInvoiceDetailTaxModel.TaxAmountFc;
                purchaseInvoiceDetailTax.TaxAmount = purchaseInvoiceDetailTaxModel.TaxAmount;
                purchaseInvoiceDetailTax.Remark = purchaseInvoiceDetailTaxModel.Remark;

                isUpdated = await Update(purchaseInvoiceDetailTax);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeletePurchaseInvoiceDetailTax(int purchaseInvoiceDetailTaxId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = await GetByIdAsync(w => w.InvoiceDetTaxId == purchaseInvoiceDetailTaxId);

            if (null != purchaseInvoiceDetailTax)
            {
                isDeleted = await Delete(purchaseInvoiceDetailTax);
            }

            return isDeleted; // returns.
        }

        public async Task<PurchaseInvoiceDetailTaxModel> GetPurchaseInvoiceDetailTaxById(int purchaseInvoiceDetailTaxId)
        {
            PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel = null;

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(purchaseInvoiceDetailTaxId, 0, 0);

            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                purchaseInvoiceDetailTaxModel = purchaseInvoiceDetailTaxModelList.FirstOrDefault();
            }

            return purchaseInvoiceDetailTaxModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxListByPurchaseInvoiceDetailId(int purchaseInvoiceDetailId)
        {
            DataTableResultModel<PurchaseInvoiceDetailTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(0, purchaseInvoiceDetailId, 0);

            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();
                resultModel.ResultList = purchaseInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxListByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceDetailTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(0, 0, purchaseInvoiceId);

            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();
                resultModel.ResultList = purchaseInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList()
        {
            DataTableResultModel<PurchaseInvoiceDetailTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(0, 0, 0);

            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();
                resultModel.ResultList = purchaseInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList(int purchaseInvoiceDetailTaxId, int purchaseInvoiceDetailId, int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = null;

            // create query.
            IQueryable<Purchaseinvoicedetailtax> query = GetQueryByCondition(w => w.InvoiceDetTaxId != 0);

            // apply filters.
            if (0 != purchaseInvoiceDetailTaxId)
                query = query.Where(w => w.InvoiceDetTaxId == purchaseInvoiceDetailTaxId);

            if (0 != purchaseInvoiceDetailId)
                query = query.Where(w => w.InvoiceDetId == purchaseInvoiceDetailId);

            if (0 != purchaseInvoiceDetailTaxId)
                query = query.Where(w => w.InvoiceDet.InvoiceId == purchaseInvoiceId);


            // get records by query.
            List<Purchaseinvoicedetailtax> purchaseInvoiceDetailTaxList = await query.ToListAsync();
            if (null != purchaseInvoiceDetailTaxList && purchaseInvoiceDetailTaxList.Count > 0)
            {
                purchaseInvoiceDetailTaxModelList = new List<PurchaseInvoiceDetailTaxModel>();

                foreach (Purchaseinvoicedetailtax purchaseInvoiceDetailTax in purchaseInvoiceDetailTaxList)
                {
                    purchaseInvoiceDetailTaxModelList.Add(await AssignValueToModel(purchaseInvoiceDetailTax));
                }
            }

            return purchaseInvoiceDetailTaxModelList; // returns.
        }

        private async Task<PurchaseInvoiceDetailTaxModel> AssignValueToModel(Purchaseinvoicedetailtax purchaseInvoiceDetailTax)
        {
            return await Task.Run(() =>
            {
                PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel = new PurchaseInvoiceDetailTaxModel();

                purchaseInvoiceDetailTaxModel.InvoiceDetTaxId = purchaseInvoiceDetailTax.InvoiceDetTaxId;
                purchaseInvoiceDetailTaxModel.InvoiceDetId = purchaseInvoiceDetailTax.InvoiceDetId;
                purchaseInvoiceDetailTaxModel.SrNo = purchaseInvoiceDetailTax.SrNo;
                purchaseInvoiceDetailTaxModel.TaxLedgerId = purchaseInvoiceDetailTax.TaxLedgerId;
                purchaseInvoiceDetailTaxModel.TaxPercentageOrAmount = purchaseInvoiceDetailTax.TaxPercentageOrAmount;
                purchaseInvoiceDetailTaxModel.TaxPerOrAmountFc = purchaseInvoiceDetailTax.TaxPerOrAmountFc;
                purchaseInvoiceDetailTaxModel.TaxAddOrDeduct = purchaseInvoiceDetailTax.TaxAddOrDeduct;
                purchaseInvoiceDetailTaxModel.TaxAmountFc = purchaseInvoiceDetailTax.TaxAmountFc;
                purchaseInvoiceDetailTaxModel.TaxAmount = purchaseInvoiceDetailTax.TaxAmount;
                purchaseInvoiceDetailTaxModel.Remark = purchaseInvoiceDetailTax.Remark;

                purchaseInvoiceDetailTaxModel.TaxLedgerName = purchaseInvoiceDetailTax.TaxLedger.LedgerName;

                return purchaseInvoiceDetailTaxModel;
            });
        }
    }
}
