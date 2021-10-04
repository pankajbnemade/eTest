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
    public class PurchaseInvoiceDetailTaxService : Repository<Purchaseinvoicedetailtax>, IPurchaseInvoiceDetailTax
    {
        IPurchaseInvoiceDetail purchaseInvoiceDetail;
        private readonly ITaxRegisterDetail taxRegisterDetail;


        public PurchaseInvoiceDetailTaxService(ErpDbContext dbContext, IPurchaseInvoiceDetail _purchaseInvoiceDetail, ITaxRegisterDetail _taxRegisterDetail) : base(dbContext)
        {
            purchaseInvoiceDetail = _purchaseInvoiceDetail;
            taxRegisterDetail = _taxRegisterDetail;
        }

        /// <summary>
        /// generate sr no based on purchaseInvoiceDetId
        /// </summary>
        /// <param name="purchaseInvoiceDetId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int purchaseInvoiceDetId)
        {
            int srNo = 0;

            if (await Any(w => w.PurchaseInvoiceDetTaxId != 0 && w.PurchaseInvoiceDetId == purchaseInvoiceDetId))
            {
                srNo = await GetQueryByCondition(w => w.PurchaseInvoiceDetTaxId != 0 && w.PurchaseInvoiceDetId == purchaseInvoiceDetId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel)
        {
            int purchaseInvoiceDetailTaxId = 0;

            //PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = null;
            //purchaseInvoiceDetailModel = await purchaseInvoiceDetail.GetPurchaseInvoiceDetailById((Int32)purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetId);

            // assign values.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = new Purchaseinvoicedetailtax();

            purchaseInvoiceDetailTax.PurchaseInvoiceDetId = purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetId;
            purchaseInvoiceDetailTax.SrNo = purchaseInvoiceDetailTaxModel.SrNo;
            purchaseInvoiceDetailTax.TaxLedgerId = purchaseInvoiceDetailTaxModel.TaxLedgerId;
            purchaseInvoiceDetailTax.TaxPercentageOrAmount = purchaseInvoiceDetailTaxModel.TaxPercentageOrAmount;
            purchaseInvoiceDetailTax.TaxPerOrAmountFc = purchaseInvoiceDetailTaxModel.TaxPerOrAmountFc;

            purchaseInvoiceDetailTax.TaxAddOrDeduct = purchaseInvoiceDetailTaxModel.TaxAddOrDeduct;
            purchaseInvoiceDetailTax.TaxAmountFc = 0;
            purchaseInvoiceDetailTax.TaxAmount = 0;
            purchaseInvoiceDetailTax.Remark = purchaseInvoiceDetailTaxModel.Remark;

            await Create(purchaseInvoiceDetailTax);

            purchaseInvoiceDetailTaxId = purchaseInvoiceDetailTax.PurchaseInvoiceDetTaxId;

            if (purchaseInvoiceDetailTaxId != 0)
            {
                await UpdatePurchaseInvoiceDetailTaxAmount(purchaseInvoiceDetailTaxId);
            }

            return purchaseInvoiceDetailTaxId; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel)
        {
            bool isUpdated = false;


            // get record.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = await GetQueryByCondition(w => w.PurchaseInvoiceDetTaxId == purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetTaxId)
                                                                .Include(w => w.PurchaseInvoiceDet).FirstOrDefaultAsync();

            if (null != purchaseInvoiceDetailTax)
            {
                // assign values.
                purchaseInvoiceDetailTax.PurchaseInvoiceDetId = purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetId;
                purchaseInvoiceDetailTax.SrNo = purchaseInvoiceDetailTaxModel.SrNo;
                purchaseInvoiceDetailTax.TaxLedgerId = purchaseInvoiceDetailTaxModel.TaxLedgerId;
                purchaseInvoiceDetailTax.TaxPercentageOrAmount = purchaseInvoiceDetailTaxModel.TaxPercentageOrAmount;
                purchaseInvoiceDetailTax.TaxPerOrAmountFc = purchaseInvoiceDetailTaxModel.TaxPerOrAmountFc;

                purchaseInvoiceDetailTax.TaxAddOrDeduct = purchaseInvoiceDetailTaxModel.TaxAddOrDeduct;
                purchaseInvoiceDetailTax.TaxAmountFc = 0;
                purchaseInvoiceDetailTax.TaxAmount = 0;
                purchaseInvoiceDetailTax.Remark = purchaseInvoiceDetailTaxModel.Remark;

                isUpdated = await Update(purchaseInvoiceDetailTax);
            }

            if (isUpdated != false)
            {
                await UpdatePurchaseInvoiceDetailTaxAmount(purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceDetailTaxAmount(int? purchaseInvoiceDetailTaxId)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = await GetQueryByCondition(w => w.PurchaseInvoiceDetTaxId == purchaseInvoiceDetailTaxId)
                                                                .Include(w => w.PurchaseInvoiceDet).ThenInclude(w => w.PurchaseInvoice).FirstOrDefaultAsync();

            if (null != purchaseInvoiceDetail)
            {
                if (DiscountType.Percentage.ToString() == purchaseInvoiceDetailTax.TaxPercentageOrAmount)
                {
                    purchaseInvoiceDetailTax.TaxAmountFc = (purchaseInvoiceDetailTax.PurchaseInvoiceDet.GrossAmountFc * purchaseInvoiceDetailTax.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    purchaseInvoiceDetailTax.TaxAmountFc = purchaseInvoiceDetailTax.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == purchaseInvoiceDetailTax.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                purchaseInvoiceDetailTax.TaxAmountFc = multiplier * purchaseInvoiceDetailTax.TaxAmountFc;

                purchaseInvoiceDetailTax.TaxAmount = purchaseInvoiceDetailTax.TaxAmountFc / purchaseInvoiceDetailTax.PurchaseInvoiceDet.PurchaseInvoice.ExchangeRate;

                isUpdated = await Update(purchaseInvoiceDetailTax);
            }

            if (isUpdated != false)
            {
                await purchaseInvoiceDetail.UpdatePurchaseInvoiceDetailAmount(purchaseInvoiceDetailTax.PurchaseInvoiceDetId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceDetailTaxAmountOnDetailUpdate(int? purchaseInvoiceDetailId)
        {
            bool isUpdated = false;

            // get record.
            IList<Purchaseinvoicedetailtax> purchaseInvoiceDetailTaxList = await GetQueryByCondition(w => w.PurchaseInvoiceDetId == (int)purchaseInvoiceDetailId).ToListAsync();

            foreach (Purchaseinvoicedetailtax purchaseInvoiceDetailTax in purchaseInvoiceDetailTaxList)
            {
                isUpdated = await UpdatePurchaseInvoiceDetailTaxAmount(purchaseInvoiceDetailTax.PurchaseInvoiceDetTaxId);
            }

            if (isUpdated != false)
            {
                await purchaseInvoiceDetail.UpdatePurchaseInvoiceDetailAmount(purchaseInvoiceDetailId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeletePurchaseInvoiceDetailTax(int purchaseInvoiceDetailTaxId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicedetailtax purchaseInvoiceDetailTax = await GetByIdAsync(w => w.PurchaseInvoiceDetTaxId == purchaseInvoiceDetailTaxId);

            if (null != purchaseInvoiceDetailTax)
            {
                isDeleted = await Delete(purchaseInvoiceDetailTax);
            }

            if (isDeleted != false)
            {
                await purchaseInvoiceDetail.UpdatePurchaseInvoiceDetailAmount(purchaseInvoiceDetailTax.PurchaseInvoiceDetId);
            }

            return isDeleted; // returns.
        }

        public async Task<bool> AddPurchaseInvoiceDetailTaxByPurchaseInvoiceId(int purchaseInvoiceId, int taxRegisterId)
        {
            bool isUpdated = false;

            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = await purchaseInvoiceDetail.GetPurchaseInvoiceDetailListByPurchaseInvoiceId(purchaseInvoiceId);

            IList<TaxRegisterDetailModel> taxRegisterDetailModelList = await taxRegisterDetail.GetTaxRegisterDetailListByTaxRegisterId(taxRegisterId);

            PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel = null;

            if (null != taxRegisterDetailModelList && taxRegisterDetailModelList.Count > 0)
            {
                foreach (TaxRegisterDetailModel taxRegisterDetailModel in taxRegisterDetailModelList)
                {
                    foreach (PurchaseInvoiceDetailModel purchaseInvoiceDetailModel in purchaseInvoiceDetailModelList)
                    {
                        purchaseInvoiceDetailTaxModel = new PurchaseInvoiceDetailTaxModel()
                        {
                            PurchaseInvoiceDetTaxId = 0,
                            PurchaseInvoiceDetId = purchaseInvoiceDetailModel.PurchaseInvoiceDetId,
                            SrNo = taxRegisterDetailModel.SrNo,
                            TaxLedgerId = taxRegisterDetailModel.TaxLedgerId,
                            TaxPercentageOrAmount = taxRegisterDetailModel.TaxPercentageOrAmount,
                            TaxPerOrAmountFc = taxRegisterDetailModel.Rate,
                            TaxAddOrDeduct = taxRegisterDetailModel.TaxAddOrDeduct,
                            TaxAmountFc = 0,
                            TaxAmount = 0,
                            Remark = ""
                        };

                        await CreatePurchaseInvoiceDetailTax(purchaseInvoiceDetailTaxModel);
                    }
                }
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeletePurchaseInvoiceDetailTaxByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            bool isDeleted = false;

            // get record.
            IList<Purchaseinvoicedetailtax> purchaseInvoiceDetailTaxList = await GetQueryByCondition(w => w.PurchaseInvoiceDetTaxId != 0).Include(W => W.PurchaseInvoiceDet)
                                                                                .Where(W => W.PurchaseInvoiceDet.PurchaseInvoiceId == purchaseInvoiceId)
                                                                                .ToListAsync();

            foreach (Purchaseinvoicedetailtax purchaseInvoiceDetailTax in purchaseInvoiceDetailTaxList)
            {
                isDeleted = await DeletePurchaseInvoiceDetailTax(purchaseInvoiceDetailTax.PurchaseInvoiceDetTaxId);
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

        public async Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxByPurchaseInvoiceDetailId(int purchaseInvoiceDetailId)
        {
            DataTableResultModel<PurchaseInvoiceDetailTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(0, purchaseInvoiceDetailId, 0);
            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();
                resultModel.ResultList = purchaseInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();
                resultModel.ResultList = new List<PurchaseInvoiceDetailTaxModel>();
                resultModel.TotalResultCount = 0;
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
            IQueryable<Purchaseinvoicedetailtax> query = GetQueryByCondition(w => w.PurchaseInvoiceDetTaxId != 0)
                                                        .Include(w => w.TaxLedger);

            // apply filters.
            if (0 != purchaseInvoiceDetailTaxId)
                query = query.Where(w => w.PurchaseInvoiceDetTaxId == purchaseInvoiceDetailTaxId);

            // apply filters.
            if (0 != purchaseInvoiceDetailId)
                query = query.Where(w => w.PurchaseInvoiceDetId == purchaseInvoiceDetailId);

            // apply filters.
            if (0 != purchaseInvoiceId)
                query = query.Where(w => w.PurchaseInvoiceDet.PurchaseInvoiceId == purchaseInvoiceId);

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

                purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetTaxId = purchaseInvoiceDetailTax.PurchaseInvoiceDetTaxId;
                purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetId = purchaseInvoiceDetailTax.PurchaseInvoiceDetId;
                purchaseInvoiceDetailTaxModel.SrNo = purchaseInvoiceDetailTax.SrNo;
                purchaseInvoiceDetailTaxModel.TaxLedgerId = purchaseInvoiceDetailTax.TaxLedgerId;
                purchaseInvoiceDetailTaxModel.TaxPercentageOrAmount = purchaseInvoiceDetailTax.TaxPercentageOrAmount;
                purchaseInvoiceDetailTaxModel.TaxPerOrAmountFc = purchaseInvoiceDetailTax.TaxPerOrAmountFc;
                purchaseInvoiceDetailTaxModel.TaxAddOrDeduct = purchaseInvoiceDetailTax.TaxAddOrDeduct;
                purchaseInvoiceDetailTaxModel.TaxAmountFc = purchaseInvoiceDetailTax.TaxAmountFc;
                purchaseInvoiceDetailTaxModel.TaxAmount = purchaseInvoiceDetailTax.TaxAmount;
                purchaseInvoiceDetailTaxModel.Remark = purchaseInvoiceDetailTax.Remark;

                purchaseInvoiceDetailTaxModel.TaxLedgerName = null != purchaseInvoiceDetailTax.TaxLedger ? purchaseInvoiceDetailTax.TaxLedger.LedgerName : null; ;

                return purchaseInvoiceDetailTaxModel;
            });
        }
    }
}
