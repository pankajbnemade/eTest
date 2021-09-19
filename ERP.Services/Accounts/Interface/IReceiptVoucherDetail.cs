using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceiptVoucherDetail : IRepository<Receiptvoucherdetail>
    {
        Task<int> CreateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel);

        Task<bool> UpdateReceiptVoucherDetail(ReceiptVoucherDetailModel receiptVoucherDetailModel);

        Task<bool> UpdateReceiptVoucherDetailAmount(int? receiptVoucherDetailId);

        Task<bool> DeleteReceiptVoucherDetail(int receiptVoucherDetailId);

        Task<ReceiptVoucherDetailModel> GetReceiptVoucherDetailById(int receiptVoucherDetailId);

        Task<IList<ReceiptVoucherDetailModel>> GetInvoiceListByParticularLedgerId(int particularLedgerId);

        Task<DataTableResultModel<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailByReceiptVoucherId(int receiptVoucherId);

        Task<DataTableResultModel<ReceiptVoucherDetailModel>> GetReceiptVoucherDetailList();

    }
}
