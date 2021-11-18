using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceiptVoucherDetail : IRepository<Receiptvoucherdetail>
    {
        Task<int> CreateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel);

        Task<bool> UpdateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel);

        Task<bool> UpdateReceiptVoucherDetailAmount(int receiptVoucherDetailId);

        Task<bool> DeleteReceiptVoucherDetail(int receiptVoucherDetailId);

        Task<ReceiptVoucherDetailModel> GetReceiptVoucherDetailById(int receiptVoucherDetailId, int receiptVoucherId);

        Task<IList<ReceiptVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailByReceiptVoucherId(int receiptVoucherId, int addRow);

        Task<IList<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailByVoucherId(int receiptVoucherId, int addRow_Blank);

        Task<AdvanceAdjustmentVoucherDetailModel> GetVoucherDetail(int receiptVoucherDetId);

        Task<IList<SelectListModel>> GetVocuherSelectList(int particularLedgerId, DateTime advanceAdjustmentDate, int voucherDetId, decimal amountFc);

        Task<decimal> GetVoucherAvailableAmount(int receiptVoucherDetId);

        Task<IList<GeneralLedgerModel>> GetTransactionList(int ledgerId, DateTime fromDate, DateTime toDate, int yearId, int companyId);
    }
}
