using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ILedgerAddress : IRepository<Ledgeraddress>
    {
        Task<int> CreateLedgerAddress(LedgerAddressModel ledgerAddressModel);

        Task<bool> UpdateLedgerAddress(LedgerAddressModel ledgerAddressModel);

        Task<bool> DeleteLedgerAddress(int ledgerAddressId);

        Task<LedgerAddressModel> GetLedgerAddressById(int ledgerAddressId);

        Task<DataTableResultModel<LedgerAddressModel>> GetLedgerAddressList();
    }
}
