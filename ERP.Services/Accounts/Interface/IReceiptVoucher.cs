using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceiptVoucher : IRepository<Receiptvoucher>
    {
        Task<GenerateNoModel> GenerateReceiptVoucherNo(int companyId, int financialYearId);

        Task<int> CreateReceiptVoucher(ReceiptVoucherModel receiptVoucherModel);

        Task<bool> UpdateReceiptVoucher(ReceiptVoucherModel receiptVoucherModel);

        Task<bool> DeleteReceiptVoucher(int receiptVoucherId);

        Task<bool> UpdateReceiptVoucherMasterAmount(int receiptVoucherId);

        Task<bool> UpdateStatusReceiptVoucher(int receiptVoucherId, int action);

        Task<ReceiptVoucherModel> GetReceiptVoucherById(int receiptVoucherId);

        Task<DataTableResultModel<ReceiptVoucherModel>> GetReceiptVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterReceiptVoucherModel searchFilterModel);

        Task<IList<SelectListModel>> GetVocuherSelectList();

        Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId);
    }
}
