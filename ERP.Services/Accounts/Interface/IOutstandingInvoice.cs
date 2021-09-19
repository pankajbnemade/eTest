using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IOutstandingInvoice
    {
        Task<DataTableResultModel<OutstandingInvoiceModel>> GetOutstandingInvoiceListByLedgerId(int ledgerId, string VoucherType, decimal ExchangeRate);
    }
}
