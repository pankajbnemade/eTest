﻿using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPurchaseInvoiceDetailTax : IRepository<Purchaseinvoicedetailtax>
    {
        Task<int> CreatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel);
       
        Task<bool> UpdatePurchaseInvoiceDetailTax(PurchaseInvoiceDetailTaxModel purchaseInvoiceDetailTaxModel);
      
        Task<bool> DeletePurchaseInvoiceDetailTax(int purchaseInvoiceDetailTaxId);
        
        Task<PurchaseInvoiceDetailTaxModel> GetPurchaseInvoiceDetailTaxById(int purchaseInvoiceDetailTaxId);

        //Task<PurchaseInvoiceDetailTaxModel> GetPurchaseInvoiceDetailTaxByPurchaseInvoiceId(int purchaseInvoiceId);

        Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList();
    }
}
