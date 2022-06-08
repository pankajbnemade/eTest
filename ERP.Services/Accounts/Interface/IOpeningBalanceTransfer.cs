using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IOpeningBalanceTransfer : IRepository<Ledgerfinancialyearbalance>
    {
        Task<bool> UpdateOpeningBalanceTransfer(OpeningBalanceTransferModel openingBalanceTransferModel);
    }
}
