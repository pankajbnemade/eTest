using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IContraVoucherDetail : IRepository<Contravoucherdetail>
    {
        Task<int> CreateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel);

        Task<bool> UpdateContraVoucherDetail(ContraVoucherDetailModel contraVoucherDetailModel);

        Task<bool> UpdateContraVoucherDetailAmount(int contraVoucherDetailId);

        Task<bool> DeleteContraVoucherDetail(int contraVoucherDetailId);

        Task<ContraVoucherDetailModel> GetContraVoucherDetailById(int contraVoucherDetailId,int contraVoucherId);

        Task<IList<ContraVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<ContraVoucherDetailModel>> GetContraVoucherDetailByContraVoucherId(int contraVoucherId, int addRow);

        Task<IList<ContraVoucherDetailModel>> GetContraVoucherDetailByVoucherId(int contraVoucherId, int addRow_Blank);

        Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId);
    }
}
