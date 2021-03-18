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

            //purchaseInvoiceDetailTax.PurchaseInvoiceDetailTaxName = purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetailTaxName;

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
                //purchaseInvoiceDetailTax.PurchaseInvoiceDetailTaxName = purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetailTaxName;

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

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(purchaseInvoiceDetailTaxId);

            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                purchaseInvoiceDetailTaxModel = purchaseInvoiceDetailTaxModelList.FirstOrDefault();
            }

            return purchaseInvoiceDetailTaxModel; // returns.
        }


        public async Task<IList<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxByStateId(int stateId)
        {
            return await GetPurchaseInvoiceDetailTaxList(0);
        }


        public async Task<DataTableResultModel<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList()
        {
            DataTableResultModel<PurchaseInvoiceDetailTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();

            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = await GetPurchaseInvoiceDetailTaxList(0);

            if (null != purchaseInvoiceDetailTaxModelList && purchaseInvoiceDetailTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailTaxModel>();
                resultModel.ResultList = purchaseInvoiceDetailTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceDetailTaxModel>> GetPurchaseInvoiceDetailTaxList(int purchaseInvoiceDetailTaxId)
        {
            IList<PurchaseInvoiceDetailTaxModel> purchaseInvoiceDetailTaxModelList = null;

            // create query.
            IQueryable<Purchaseinvoicedetailtax> query = GetQueryByCondition(w => w.InvoiceDetTaxId != 0);

            // apply filters.
            if (0 != purchaseInvoiceDetailTaxId)
                query = query.Where(w => w.InvoiceDetTaxId == purchaseInvoiceDetailTaxId);

          

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

                //purchaseInvoiceDetailTaxModel.InvoiceDetTaxId = purchaseInvoiceDetailTax.InvoiceDetTaxId;
                //purchaseInvoiceDetailTaxModel.PurchaseInvoiceDetailTaxName = purchaseInvoiceDetailTax.PurchaseInvoiceDetailTaxName;

                return purchaseInvoiceDetailTaxModel;
            });
        }
    }
}
