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
    public class PurchaseInvoiceDetailService : Repository<Purchaseinvoicedetail>, IPurchaseInvoiceDetail
    {
        public PurchaseInvoiceDetailService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel)
        {
            int purchaseInvoiceDetailId = 0;

            // assign values.
            Purchaseinvoicedetail purchaseInvoiceDetail = new Purchaseinvoicedetail();

            //purchaseInvoiceDetail.PurchaseInvoiceDetailName = purchaseInvoiceDetailModel.PurchaseInvoiceDetailName;

            purchaseInvoiceDetailId = await Create(purchaseInvoiceDetail);

            return purchaseInvoiceDetailId; // returns.
        }


        public async Task<bool> UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetailModel purchaseInvoiceDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Purchaseinvoicedetail purchaseInvoiceDetail = await GetByIdAsync(w => w.InvoiceDetId == purchaseInvoiceDetailModel.InvoiceDetId);

            if (null != purchaseInvoiceDetail)
            {
                // assign values.
                //purchaseInvoiceDetail.PurchaseInvoiceDetailName = purchaseInvoiceDetailModel.PurchaseInvoiceDetailName;

                isUpdated = await Update(purchaseInvoiceDetail);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeletePurchaseInvoiceDetail(int purchaseInvoiceDetailId)
        {
            bool isDeleted = false;

            // get record.
            Purchaseinvoicedetail purchaseInvoiceDetail = await GetByIdAsync(w => w.InvoiceDetId == purchaseInvoiceDetailId);

            if (null != purchaseInvoiceDetail)
            {
                isDeleted = await Delete(purchaseInvoiceDetail);
            }

            return isDeleted; // returns.
        }


        public async Task<PurchaseInvoiceDetailModel> GetPurchaseInvoiceDetailById(int purchaseInvoiceDetailId)
        {
            PurchaseInvoiceDetailModel purchaseInvoiceDetailModel = null;

            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = await GetPurchaseInvoiceDetailList(purchaseInvoiceDetailId);

            if (null != purchaseInvoiceDetailModelList && purchaseInvoiceDetailModelList.Any())
            {
                purchaseInvoiceDetailModel = purchaseInvoiceDetailModelList.FirstOrDefault();
            }

            return purchaseInvoiceDetailModel; // returns.
        }


        public async Task<IList<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailByStateId(int stateId)
        {
            return await GetPurchaseInvoiceDetailList(0);
        }


        public async Task<DataTableResultModel<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailList()
        {
            DataTableResultModel<PurchaseInvoiceDetailModel> resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();

            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = await GetPurchaseInvoiceDetailList(0);

            if (null != purchaseInvoiceDetailModelList && purchaseInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<PurchaseInvoiceDetailModel>();
                resultModel.ResultList = purchaseInvoiceDetailModelList;
                resultModel.TotalResultCount = purchaseInvoiceDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<PurchaseInvoiceDetailModel>> GetPurchaseInvoiceDetailList(int purchaseInvoiceDetailId)
        {
            IList<PurchaseInvoiceDetailModel> purchaseInvoiceDetailModelList = null;

            // create query.
            IQueryable<Purchaseinvoicedetail> query = GetQueryByCondition(w => w.InvoiceDetId != 0);

            // apply filters.
            if (0 != purchaseInvoiceDetailId)
                query = query.Where(w => w.InvoiceDetId == purchaseInvoiceDetailId);



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

                //purchaseInvoiceDetailModel.InvoiceDetId = purchaseInvoiceDetail.InvoiceDetId;
                //purchaseInvoiceDetailModel.PurchaseInvoiceDetailName = purchaseInvoiceDetail.PurchaseInvoiceDetailName;

                return purchaseInvoiceDetailModel;
            });
        }
    }
}
