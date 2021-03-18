using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceDetail : IRepository<Salesinvoicedetail>
    {
        Task<int> CreateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel);

        Task<bool> UpdateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel);

        Task<bool> DeleteSalesInvoiceDetail(int salesInvoiceDetailId);

        Task<SalesInvoiceDetailModel> GetSalesInvoiceDetailById(int salesInvoiceDetailId);

        //Task<SalesInvoiceDetailModel> GetSalesInvoiceDetailBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList();
    }
}
