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
    public class SalesInvoiceDetailService : Repository<Salesinvoicedetail>, ISalesInvoiceDetail
    {
        public SalesInvoiceDetailService(ErpDbContext dbContext) : base(dbContext) { }


        public async Task<int> CreateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            int salesInvoiceDetailId = 0;

            // assign values.
            Salesinvoicedetail salesInvoiceDetail = new Salesinvoicedetail();

            //salesInvoiceDetail.SalesInvoiceDetailName = salesInvoiceDetailModel.SalesInvoiceDetailName;

            salesInvoiceDetailId = await Create(salesInvoiceDetail);

            return salesInvoiceDetailId; // returns.
        }


        public async Task<bool> UpdateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel)
        {
            bool isUpdated = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetByIdAsync(w => w.InvoiceDetId == salesInvoiceDetailModel.InvoiceDetId);

            if (null != salesInvoiceDetail)
            {
                // assign values.
                //salesInvoiceDetail.SalesInvoiceDetailName = salesInvoiceDetailModel.SalesInvoiceDetailName;

                isUpdated = await Update(salesInvoiceDetail);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteSalesInvoiceDetail(int salesInvoiceDetailId)
        {
            bool isDeleted = false;

            // get record.
            Salesinvoicedetail salesInvoiceDetail = await GetByIdAsync(w => w.InvoiceDetId == salesInvoiceDetailId);

            if (null != salesInvoiceDetail)
            {
                isDeleted = await Delete(salesInvoiceDetail);
            }

            return isDeleted; // returns.
        }


        public async Task<SalesInvoiceDetailModel> GetSalesInvoiceDetailById(int salesInvoiceDetailId)
        {
            SalesInvoiceDetailModel salesInvoiceDetailModel = null;

            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await GetSalesInvoiceDetailList(salesInvoiceDetailId);

            if (null != salesInvoiceDetailModelList && salesInvoiceDetailModelList.Any())
            {
                salesInvoiceDetailModel = salesInvoiceDetailModelList.FirstOrDefault();
            }

            return salesInvoiceDetailModel; // returns.
        }


        public async Task<IList<SalesInvoiceDetailModel>> GetSalesInvoiceDetailByStateId(int stateId)
        {
            return await GetSalesInvoiceDetailList(0);
        }


        public async Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList()
        {
            DataTableResultModel<SalesInvoiceDetailModel> resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();

            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = await GetSalesInvoiceDetailList(0);

            if (null != salesInvoiceDetailModelList && salesInvoiceDetailModelList.Any())
            {
                resultModel = new DataTableResultModel<SalesInvoiceDetailModel>();
                resultModel.ResultList = salesInvoiceDetailModelList;
                resultModel.TotalResultCount = salesInvoiceDetailModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList(int salesInvoiceDetailId)
        {
            IList<SalesInvoiceDetailModel> salesInvoiceDetailModelList = null;

            // create query.
            IQueryable<Salesinvoicedetail> query = GetQueryByCondition(w => w.InvoiceDetId != 0);

            // apply filters.
            if (0 != salesInvoiceDetailId)
                query = query.Where(w => w.InvoiceDetId == salesInvoiceDetailId);



            // get records by query.
            List<Salesinvoicedetail> salesInvoiceDetailList = await query.ToListAsync();
            if (null != salesInvoiceDetailList && salesInvoiceDetailList.Count > 0)
            {
                salesInvoiceDetailModelList = new List<SalesInvoiceDetailModel>();

                foreach (Salesinvoicedetail salesInvoiceDetail in salesInvoiceDetailList)
                {
                    salesInvoiceDetailModelList.Add(await AssignValueToModel(salesInvoiceDetail));
                }
            }

            return salesInvoiceDetailModelList; // returns.
        }

        private async Task<SalesInvoiceDetailModel> AssignValueToModel(Salesinvoicedetail salesInvoiceDetail)
        {
            return await Task.Run(() =>
            {
                SalesInvoiceDetailModel salesInvoiceDetailModel = new SalesInvoiceDetailModel();

                //salesInvoiceDetailModel.InvoiceDetId = salesInvoiceDetail.InvoiceDetId;
                //salesInvoiceDetailModel.SalesInvoiceDetailName = salesInvoiceDetail.SalesInvoiceDetailName;

                return salesInvoiceDetailModel;
            });
        }
    }
}
