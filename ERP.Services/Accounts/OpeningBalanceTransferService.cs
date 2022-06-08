using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Accounts.Enums;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using ERP.Services.Accounts.Interface;
using ERP.Services.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace ERP.Services.Accounts
{
    public class OpeningBalanceTransferService : Repository<Ledgerfinancialyearbalance>, IOpeningBalanceTransfer
    {
        public OpeningBalanceTransferService(ErpDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> UpdateOpeningBalanceTransfer(OpeningBalanceTransferModel openingBalanceTransferModel)
        {
            bool isUpdated = false;

            // get record.
            Ledgerfinancialyearbalance ledgerFinancialYearBalance = await GetByIdAsync(w => w.FinancialYearId == openingBalanceTransferModel.ToYearId);

            if (null != openingBalanceTransferModel)
            {
                isUpdated = await Update(ledgerFinancialYearBalance);
            }

            return isUpdated; // returns.
        }

    }
}
