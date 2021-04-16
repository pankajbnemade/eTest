using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceDetail : IRepository<Salesinvoicedetail>
    {
        /// <summary>
        /// generate sr no based on salesInvoiceId
        /// </summary>
        /// <param name="salesInvoiceId"></param>
        /// <returns>
        /// return sr no.
        /// </returns>
        Task<int> GenerateSrNo(int salesInvoiceId);

        Task<int> CreateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel);

        Task<bool> UpdateSalesInvoiceDetail(SalesInvoiceDetailModel salesInvoiceDetailModel);

        Task<bool> UpdateSalesInvoiceDetailAmount(int? salesInvoiceDetailId);

        Task<bool> DeleteSalesInvoiceDetail(int salesInvoiceDetailId);

        Task<SalesInvoiceDetailModel> GetSalesInvoiceDetailById(int salesInvoiceDetailId);

        Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailBySalesInvoiceId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceDetailModel>> GetSalesInvoiceDetailList();
    }
}
