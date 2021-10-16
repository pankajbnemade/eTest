using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IOutstandingInvoice
    {
        Task<IList<OutstandingInvoiceModel>> GetOutstandingInvoiceListByLedgerId(int ledgerId, string voucherType, int voucherId, DateTime voucherDate, decimal exchangeRate);
    }
}
