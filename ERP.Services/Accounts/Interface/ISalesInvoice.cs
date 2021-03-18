using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ISalesInvoice: IRepository<Salesinvoice>
    {
        Task<int> CreateSalesInvoice(SalesInvoiceModel salesInvoiceModel);

        Task<bool> UpdateSalesInvoice(SalesInvoiceModel salesInvoiceModel);

        Task<bool> DeleteSalesInvoice(int salesInvoiceId);

        Task<SalesInvoiceModel> GetSalesInvoiceById(int salesInvoiceId);

        //Task<DataTableResultModel<SalesInvoiceModel>> GetSalesInvoiceList();
    }
}
