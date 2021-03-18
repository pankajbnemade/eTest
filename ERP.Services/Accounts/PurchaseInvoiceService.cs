using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Accounts
{
    public class PurchaseInvoiceService : Repository<Purchaseinvoice>, IPurchaseInvoice
    {
        public PurchaseInvoiceService(ErpDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// create new purchase invoice.
        /// </summary>
        /// <param name="purchaseInvoiceModel"></param>
        /// <returns>
        /// return id.
        /// </returns>
        public async Task<int> CreatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            int purchaseInvoiceId = 0;

            // assign values.
            Purchaseinvoice purchaseInvoice = new Purchaseinvoice();
           
            purchaseInvoiceId = await Create(purchaseInvoice);

            return purchaseInvoiceId; // returns.
        }

        /// <summary>
        /// update purchase invoice.
        /// </summary>
        /// <param name="purchaseInvoiceModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdatePurchaseInvoice(PurchaseInvoiceModel purchaseInvoiceModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.InvoiceId == purchaseInvoiceModel.InvoiceId);
            if (null != purchaseInvoice)
            {
                // assign values.
                
                isUpdated = await Update(purchaseInvoice);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// delete purchase invoice.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeletePurchaseInvoice(int invoiceId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoice purchaseInvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != purchaseInvoice)
            {
                isDeleted = await Delete(purchaseInvoice);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get purchase invoice based on invoiceId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<PurchaseInvoiceModel> GetPurchaseInvoiceById(int invoiceId)
        {
            PurchaseInvoiceModel purchaseInvoiceModel = null;

            // get record.
            Purchaseinvoice purchaseinvoice = await GetByIdAsync(w => w.InvoiceId == invoiceId);
            if (null != purchaseInvoiceModel)
            {
                // assign values.
                purchaseInvoiceModel.InvoiceId = purchaseinvoice.InvoiceId;
               
            }

            return purchaseInvoiceModel; // returns.
        }
    }
}
