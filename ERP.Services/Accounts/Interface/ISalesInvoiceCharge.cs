using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoiceCharge : IRepository<Salesinvoicecharge>
    {
        Task<int> CreateSalesInvoiceCharge(SalesInvoiceChargeModel salesInvoiceChargeModel);
       
        Task<bool> UpdateSalesInvoiceCharge(SalesInvoiceChargeModel salesInvoiceChargeModel);
      
        Task<bool> DeleteSalesInvoiceCharge(int salesInvoiceChargeId);
        
        Task<SalesInvoiceChargeModel> GetSalesInvoiceChargeById(int salesInvoiceChargeId);

        //Task<SalesInvoiceChargeModel> GetSalesInvoiceChargeBysId(int salesInvoiceId);

        Task<DataTableResultModel<SalesInvoiceChargeModel>> GetSalesInvoiceChargeList();
    }
}
