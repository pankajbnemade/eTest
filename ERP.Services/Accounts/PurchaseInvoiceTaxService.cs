﻿using ERP.DataAccess.EntityData;
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
    public class PurchaseInvoiceTaxService : Repository<Purchaseinvoicetax>, IPurchaseInvoiceTax
    {
        private readonly IPurchaseInvoice purchaseInvoice;

        public PurchaseInvoiceTaxService(ErpDbContext dbContext, IPurchaseInvoice _purchaseInvoice) : base(dbContext)
        {
            purchaseInvoice = _purchaseInvoice;
        }

        /// <summary>
        /// generate sr no based on purchaseInvoiceId
        /// </summary>
        /// <param name="purchaseInvoiceId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        public async Task<int> GenerateSrNo(int purchaseInvoiceId)
        {
            int srNo = 0;

            if (await Any(w => w.PurchaseInvoiceTaxId != 0 && w.PurchaseInvoiceId == purchaseInvoiceId))
            {
                srNo = await GetQueryByCondition(w => w.PurchaseInvoiceTaxId != 0 && w.PurchaseInvoiceId == purchaseInvoiceId).MaxAsync(m => Convert.ToInt32(m.SrNo));
            }

            return (srNo + 1);
        }

        public async Task<int> CreatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel)
        {
            int purchaseInvoiceTaxId = 0;
            int multiplier = 1;

            // assign values.
            PurchaseInvoiceModel purchaseInvoiceModel = null;

            purchaseInvoiceModel = await purchaseInvoice.GetPurchaseInvoiceById((int)purchaseInvoiceTaxModel.PurchaseInvoiceId);

            Purchaseinvoicetax purchaseInvoiceTax = new Purchaseinvoicetax();
            purchaseInvoiceTax.PurchaseInvoiceId = purchaseInvoiceTaxModel.PurchaseInvoiceId;
            purchaseInvoiceTax.SrNo = purchaseInvoiceTaxModel.SrNo;
            purchaseInvoiceTax.TaxLedgerId = purchaseInvoiceTaxModel.TaxLedgerId;
            purchaseInvoiceTax.TaxPercentageOrAmount = purchaseInvoiceTaxModel.TaxPercentageOrAmount;
            purchaseInvoiceTax.TaxPerOrAmountFc = purchaseInvoiceTaxModel.TaxPerOrAmountFc;

            if (DiscountType.Percentage.ToString() == purchaseInvoiceTax.TaxPercentageOrAmount)
            {
                purchaseInvoiceTaxModel.TaxAmountFc = (purchaseInvoiceModel.GrossAmountFc * purchaseInvoiceTaxModel.TaxPerOrAmountFc) / 100;
            }
            else
            {
                purchaseInvoiceTaxModel.TaxAmountFc = purchaseInvoiceTaxModel.TaxPerOrAmountFc;
            }

            if (TaxAddOrDeduct.Deduct.ToString() == purchaseInvoiceTaxModel.TaxAddOrDeduct)
            {
                multiplier = -1;
            }

            purchaseInvoiceTax.TaxAddOrDeduct = purchaseInvoiceTaxModel.TaxAddOrDeduct;
            purchaseInvoiceTax.TaxAmountFc = multiplier * purchaseInvoiceTaxModel.TaxAmountFc;
            purchaseInvoiceTax.TaxAmount = 0;
            purchaseInvoiceTax.Remark = purchaseInvoiceTaxModel.Remark;

            await Create(purchaseInvoiceTax);
            purchaseInvoiceTaxId = purchaseInvoiceTax.PurchaseInvoiceTaxId;

            if (purchaseInvoiceTaxId != 0)
            {
                await UpdatePurchaseInvoiceTaxAmount(purchaseInvoiceTaxId);;
            }

            return purchaseInvoiceTaxId; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel)
        {
            bool isUpdated = false;
            int multiplier = 1;

            // get record.
            Purchaseinvoicetax purchaseInvoiceTax = await GetQueryByCondition(w => w.PurchaseInvoiceTaxId == purchaseInvoiceTaxModel.PurchaseInvoiceTaxId)
                        .Include(w => w.PurchaseInvoice).FirstOrDefaultAsync();

            if (null != purchaseInvoiceTax)
            {
                // assign values.

                purchaseInvoiceTax.PurchaseInvoiceId = purchaseInvoiceTaxModel.PurchaseInvoiceId;
                purchaseInvoiceTax.SrNo = purchaseInvoiceTaxModel.SrNo;
                purchaseInvoiceTax.TaxLedgerId = purchaseInvoiceTaxModel.TaxLedgerId;
                purchaseInvoiceTax.TaxPercentageOrAmount = purchaseInvoiceTaxModel.TaxPercentageOrAmount;
                purchaseInvoiceTax.TaxPerOrAmountFc = purchaseInvoiceTaxModel.TaxPerOrAmountFc;

                if (DiscountType.Percentage.ToString() == purchaseInvoiceTax.TaxPercentageOrAmount)
                {
                    purchaseInvoiceTaxModel.TaxAmountFc = (purchaseInvoiceTax.PurchaseInvoice.GrossAmountFc * purchaseInvoiceTaxModel.TaxPerOrAmountFc) / 100;
                }
                else
                {
                    purchaseInvoiceTaxModel.TaxAmountFc = purchaseInvoiceTaxModel.TaxPerOrAmountFc;
                }

                if (TaxAddOrDeduct.Deduct.ToString() == purchaseInvoiceTaxModel.TaxAddOrDeduct)
                {
                    multiplier = -1;
                }

                purchaseInvoiceTax.TaxAddOrDeduct = purchaseInvoiceTaxModel.TaxAddOrDeduct;
                purchaseInvoiceTax.TaxAmountFc = multiplier * purchaseInvoiceTaxModel.TaxAmountFc;
                purchaseInvoiceTax.TaxAmount = 0;
                purchaseInvoiceTax.Remark = purchaseInvoiceTaxModel.Remark;

                isUpdated = await Update(purchaseInvoiceTax);
            }

            if (isUpdated != false)
            {
                await UpdatePurchaseInvoiceTaxAmount(purchaseInvoiceTaxModel.PurchaseInvoiceTaxId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> UpdatePurchaseInvoiceTaxAmount(int? purchaseInvoiceTaxId)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicetax purchaseInvoiceTax = await GetQueryByCondition(w => w.PurchaseInvoiceTaxId == purchaseInvoiceTaxId)
                                                                 .Include(w => w.PurchaseInvoice).FirstOrDefaultAsync();

            if (null != purchaseInvoice)
            {
                purchaseInvoiceTax.TaxAmount = purchaseInvoiceTax.TaxAmountFc / purchaseInvoiceTax.PurchaseInvoice.ExchangeRate;

                isUpdated = await Update(purchaseInvoiceTax);
            }

            if (isUpdated != false)
            {
                await purchaseInvoice.UpdatePurchaseInvoiceMasterAmount(purchaseInvoiceTax.PurchaseInvoiceId);
            }

            return isUpdated; // returns.
        }

        public async Task<bool> DeletePurchaseInvoiceTax(int purchaseInvoiceTaxId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicetax purchaseInvoiceTax = await GetByIdAsync(w => w.PurchaseInvoiceTaxId == purchaseInvoiceTaxId);

            if (null != purchaseInvoiceTax)
            {
                isDeleted = await Delete(purchaseInvoiceTax);
            }

            if (isDeleted != false)
            {
                await purchaseInvoice.UpdatePurchaseInvoiceMasterAmount(purchaseInvoiceTax.PurchaseInvoiceId);
            }

            return isDeleted; // returns.
        }

        public async Task<PurchaseInvoiceTaxModel> GetPurchaseInvoiceTaxById(int purchaseInvoiceTaxId)
        {
            PurchaseInvoiceTaxModel purchaseInvoiceTaxModel = null;

            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = await GetPurchaseInvoiceTaxList(purchaseInvoiceTaxId, 0);

            if (null != purchaseInvoiceTaxModelList && purchaseInvoiceTaxModelList.Any())
            {
                purchaseInvoiceTaxModel = purchaseInvoiceTaxModelList.FirstOrDefault();
            }

            return purchaseInvoiceTaxModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxByPurchaseInvoiceId(int purchaseInvoiceId)
        {
            DataTableResultModel<PurchaseInvoiceTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();

            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = await GetPurchaseInvoiceTaxList(0, purchaseInvoiceId);

            if (null != purchaseInvoiceTaxModelList && purchaseInvoiceTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();
                resultModel.ResultList = purchaseInvoiceTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceTaxModelList.Count();
            }
            else
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();
                resultModel.ResultList = new List<PurchaseInvoiceTaxModel>();
                resultModel.TotalResultCount = 0;
            }

            return resultModel; // returns.
        }

        public async Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxList()
        {
            DataTableResultModel<PurchaseInvoiceTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();

            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = await GetPurchaseInvoiceTaxList(0, 0);

            if (null != purchaseInvoiceTaxModelList && purchaseInvoiceTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();
                resultModel.ResultList = purchaseInvoiceTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxList(int purchaseInvoiceTaxId, int purchaseInvoiceId)
        {
            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = null;

            // create query.
            IQueryable<Purchaseinvoicetax> query = GetQueryByCondition(w => w.PurchaseInvoiceTaxId != 0).Include(t => t.TaxLedger);

            // apply filters.
            if (0 != purchaseInvoiceTaxId)
                query = query.Where(w => w.PurchaseInvoiceTaxId == purchaseInvoiceTaxId);

            // apply filters.
            if (0 != purchaseInvoiceId)
                query = query.Where(w => w.PurchaseInvoiceId == purchaseInvoiceId);

            // get records by query.
            List<Purchaseinvoicetax> purchaseInvoiceTaxList = await query.ToListAsync();
            if (null != purchaseInvoiceTaxList && purchaseInvoiceTaxList.Count > 0)
            {
                purchaseInvoiceTaxModelList = new List<PurchaseInvoiceTaxModel>();

                foreach (Purchaseinvoicetax purchaseInvoiceTax in purchaseInvoiceTaxList)
                {
                    purchaseInvoiceTaxModelList.Add(await AssignValueToModel(purchaseInvoiceTax));
                }
            }

            return purchaseInvoiceTaxModelList; // returns.
        }

        private async Task<PurchaseInvoiceTaxModel> AssignValueToModel(Purchaseinvoicetax purchaseInvoiceTax)
        {
            return await Task.Run(() =>
            {
                PurchaseInvoiceTaxModel purchaseInvoiceTaxModel = new PurchaseInvoiceTaxModel();

                purchaseInvoiceTaxModel.PurchaseInvoiceTaxId = purchaseInvoiceTax.PurchaseInvoiceTaxId;
                purchaseInvoiceTaxModel.PurchaseInvoiceId = purchaseInvoiceTax.PurchaseInvoiceId;
                purchaseInvoiceTaxModel.SrNo = purchaseInvoiceTax.SrNo;
                purchaseInvoiceTaxModel.TaxLedgerId = purchaseInvoiceTax.TaxLedgerId;
                purchaseInvoiceTaxModel.TaxPercentageOrAmount = purchaseInvoiceTax.TaxPercentageOrAmount;
                purchaseInvoiceTaxModel.TaxPerOrAmountFc = purchaseInvoiceTax.TaxPerOrAmountFc;
                purchaseInvoiceTaxModel.TaxAddOrDeduct = purchaseInvoiceTax.TaxAddOrDeduct;
                purchaseInvoiceTaxModel.TaxAmountFc = purchaseInvoiceTax.TaxAmountFc;
                purchaseInvoiceTaxModel.TaxAmount = purchaseInvoiceTax.TaxAmount;
                purchaseInvoiceTaxModel.Remark = purchaseInvoiceTax.Remark;

                purchaseInvoiceTaxModel.TaxLedgerName = null != purchaseInvoiceTax.TaxLedger ? purchaseInvoiceTax.TaxLedger.LedgerName : null;

                return purchaseInvoiceTaxModel;
            });
        }
    }
}
