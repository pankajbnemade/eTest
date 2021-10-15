using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPaymentVoucher : IRepository<Paymentvoucher>
    {
        Task<GenerateNoModel> GeneratePaymentVoucherNo(int companyId, int financialYearId);

        Task<int> CreatePaymentVoucher(PaymentVoucherModel paymentVoucherModel);

        Task<bool> UpdatePaymentVoucher(PaymentVoucherModel paymentVoucherModel);

        Task<bool> DeletePaymentVoucher(int paymentVoucherId);

        Task<bool> UpdatePaymentVoucherMasterAmount(int paymentVoucherId);

         Task<bool> UpdateStatusPaymentVoucher(int paymentVoucherId, int action);

        Task<PaymentVoucherModel> GetPaymentVoucherById(int paymentVoucherId);
        
        Task<DataTableResultModel<PaymentVoucherModel>> GetPaymentVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterPaymentVoucherModel searchFilterModel);
   
    }
}
