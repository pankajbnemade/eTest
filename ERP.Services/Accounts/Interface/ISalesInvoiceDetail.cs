using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceDetail : IRepository<Salesinvoicedetail>
    {
        Task<int> CreateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel);

        Task<bool> UpdateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel);

        Task<bool> UpdateSalesInvoiceDetailAmount(int? salesInvoiceDetailId);

        Task<bool> DeleteSalesInvoiceDetail(int salesInvoiceDetailId);

        Task<SalesInvoiceDetailModel> GetSalesInvoiceDetailById(int salesInvoiceDetailId);
        Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailBySalesInvoiceId(int salesInvoiceId);
        Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList();
    }
}
