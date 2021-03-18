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
    public class PurchaseInvoiceTaxService : Repository<Purchaseinvoicetax>, IPurchaseInvoiceTax
    {
        public PurchaseInvoiceTaxService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel)
        {
            int purchaseInvoiceTaxId = 0;

            // assign values.
            Purchaseinvoicetax purchaseInvoiceTax = new Purchaseinvoicetax();

            //purchaseInvoiceTax.PurchaseInvoiceTaxName = purchaseInvoiceTaxModel.PurchaseInvoiceTaxName;

            purchaseInvoiceTaxId = await Create(purchaseInvoiceTax);

            return purchaseInvoiceTaxId; // returns.
        }


        public async Task<bool> UpdatePurchaseInvoiceTax(PurchaseInvoiceTaxModel purchaseInvoiceTaxModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicetax purchaseInvoiceTax = await GetByIdAsync(w => w.InvoiceTaxId == purchaseInvoiceTaxModel.InvoiceTaxId);

            if (null != purchaseInvoiceTax)
            {
                // assign values.
                //purchaseInvoiceTax.PurchaseInvoiceTaxName = purchaseInvoiceTaxModel.PurchaseInvoiceTaxName;

                isUpdated = await Update(purchaseInvoiceTax);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeletePurchaseInvoiceTax(int purchaseInvoiceTaxId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicetax purchaseInvoiceTax = await GetByIdAsync(w => w.InvoiceTaxId == purchaseInvoiceTaxId);

            if (null != purchaseInvoiceTax)
            {
                isDeleted = await Delete(purchaseInvoiceTax);
            }

            return isDeleted; // returns.
        }


        public async Task<PurchaseInvoiceTaxModel> GetPurchaseInvoiceTaxById(int purchaseInvoiceTaxId)
        {
            PurchaseInvoiceTaxModel purchaseInvoiceTaxModel = null;

            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = await GetPurchaseInvoiceTaxList(purchaseInvoiceTaxId);

            if (null != purchaseInvoiceTaxModelList && purchaseInvoiceTaxModelList.Any())
            {
                purchaseInvoiceTaxModel = purchaseInvoiceTaxModelList.FirstOrDefault();
            }

            return purchaseInvoiceTaxModel; // returns.
        }


        public async Task<IList<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxByStateId(int stateId)
        {
            return await GetPurchaseInvoiceTaxList(0);
        }


        public async Task<DataTableResultModel<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxList()
        {
            DataTableResultModel<PurchaseInvoiceTaxModel> resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();

            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = await GetPurchaseInvoiceTaxList(0);

            if (null != purchaseInvoiceTaxModelList && purchaseInvoiceTaxModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceTaxModel>();
                resultModel.ResultList = purchaseInvoiceTaxModelList;
                resultModel.TotalResultCount = purchaseInvoiceTaxModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceTaxModel>> GetPurchaseInvoiceTaxList(int purchaseInvoiceTaxId)
        {
            IList<PurchaseInvoiceTaxModel> purchaseInvoiceTaxModelList = null;

            // create query.
            IQueryable<Purchaseinvoicetax> query = GetQueryByCondition(w => w.InvoiceTaxId != 0);

            // apply filters.
            if (0 != purchaseInvoiceTaxId)
                query = query.Where(w => w.InvoiceTaxId == purchaseInvoiceTaxId);

          

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

                //purchaseInvoiceTaxModel.InvoiceTaxId = purchaseInvoiceTax.InvoiceTaxId;
                //purchaseInvoiceTaxModel.PurchaseInvoiceTaxName = purchaseInvoiceTax.PurchaseInvoiceTaxName;

                return purchaseInvoiceTaxModel;
            });
        }
    }
}
