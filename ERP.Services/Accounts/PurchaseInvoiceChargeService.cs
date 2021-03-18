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
    public class PurchaseInvoiceChargeService : Repository<Purchaseinvoicecharge>, IPurchaseInvoiceCharge
    {
        public PurchaseInvoiceChargeService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreatePurchaseInvoiceCharge(PurchaseInvoiceChargeModel purchaseInvoiceChargeModel)
        {
            int purchaseInvoiceChargeId = 0;

            // assign values.
            Purchaseinvoicecharge purchaseInvoiceCharge = new Purchaseinvoicecharge();

            //purchaseInvoiceCharge.PurchaseInvoiceChargeName = purchaseInvoiceChargeModel.PurchaseInvoiceChargeName;

            purchaseInvoiceChargeId = await Create(purchaseInvoiceCharge);

            return purchaseInvoiceChargeId; // returns.
        }


        public async Task<bool> UpdatePurchaseInvoiceCharge(PurchaseInvoiceChargeModel purchaseInvoiceChargeModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicecharge purchaseInvoiceCharge = await GetByIdAsync(w => w.InvoiceChargeId == purchaseInvoiceChargeModel.InvoiceChargeId);

            if (null != purchaseInvoiceCharge)
            {
                // assign values.
                //purchaseInvoiceCharge.PurchaseInvoiceChargeName = purchaseInvoiceChargeModel.PurchaseInvoiceChargeName;

                isUpdated = await Update(purchaseInvoiceCharge);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeletePurchaseInvoiceCharge(int purchaseInvoiceChargeId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicecharge purchaseInvoiceCharge = await GetByIdAsync(w => w.InvoiceChargeId == purchaseInvoiceChargeId);

            if (null != purchaseInvoiceCharge)
            {
                isDeleted = await Delete(purchaseInvoiceCharge);
            }

            return isDeleted; // returns.
        }


        public async Task<PurchaseInvoiceChargeModel> GetPurchaseInvoiceChargeById(int purchaseInvoiceChargeId)
        {
            PurchaseInvoiceChargeModel purchaseInvoiceChargeModel = null;

            IList<PurchaseInvoiceChargeModel> purchaseInvoiceChargeModelList = await GetPurchaseInvoiceChargeList(purchaseInvoiceChargeId);

            if (null != purchaseInvoiceChargeModelList && purchaseInvoiceChargeModelList.Any())
            {
                purchaseInvoiceChargeModel = purchaseInvoiceChargeModelList.FirstOrDefault();
            }

            return purchaseInvoiceChargeModel; // returns.
        }


        public async Task<IList<PurchaseInvoiceChargeModel>> GetPurchaseInvoiceChargeByStateId(int stateId)
        {
            return await GetPurchaseInvoiceChargeList(0);
        }


        public async Task<DataTableResultModel<PurchaseInvoiceChargeModel>> GetPurchaseInvoiceChargeList()
        {
            DataTableResultModel<PurchaseInvoiceChargeModel> resultModel = new DataTableResultModel<PurchaseInvoiceChargeModel>();

            IList<PurchaseInvoiceChargeModel> purchaseInvoiceChargeModelList = await GetPurchaseInvoiceChargeList(0);

            if (null != purchaseInvoiceChargeModelList && purchaseInvoiceChargeModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceChargeModel>();
                resultModel.ResultList = purchaseInvoiceChargeModelList;
                resultModel.TotalResultCount = purchaseInvoiceChargeModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceChargeModel>> GetPurchaseInvoiceChargeList(int purchaseInvoiceChargeId)
        {
            IList<PurchaseInvoiceChargeModel> purchaseInvoiceChargeModelList = null;

            // create query.
            IQueryable<Purchaseinvoicecharge> query = GetQueryByCondition(w => w.InvoiceChargeId != 0);

            // apply filters.
            if (0 != purchaseInvoiceChargeId)
                query = query.Where(w => w.InvoiceChargeId == purchaseInvoiceChargeId);

          

            // get records by query.
            List<Purchaseinvoicecharge> purchaseInvoiceChargeList = await query.ToListAsync();
            if (null != purchaseInvoiceChargeList && purchaseInvoiceChargeList.Count > 0)
            {
                purchaseInvoiceChargeModelList = new List<PurchaseInvoiceChargeModel>();

                foreach (Purchaseinvoicecharge purchaseInvoiceCharge in purchaseInvoiceChargeList)
                {
                    purchaseInvoiceChargeModelList.Add(await AssignValueToModel(purchaseInvoiceCharge));
                }
            }

            return purchaseInvoiceChargeModelList; // returns.
        }

        private async Task<PurchaseInvoiceChargeModel> AssignValueToModel(Purchaseinvoicecharge purchaseInvoiceCharge)
        {
            return await Task.Run(() =>
            {
                PurchaseInvoiceChargeModel purchaseInvoiceChargeModel = new PurchaseInvoiceChargeModel();

                //purchaseInvoiceChargeModel.InvoiceChargeId = purchaseInvoiceCharge.InvoiceChargeId;
                //purchaseInvoiceChargeModel.PurchaseInvoiceChargeName = purchaseInvoiceCharge.PurchaseInvoiceChargeName;

                return purchaseInvoiceChargeModel;
            });
        }
    }
}
