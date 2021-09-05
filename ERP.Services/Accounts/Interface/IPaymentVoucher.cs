using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IPaymentVoucher : IRepository<Paymentvoucher>
    {
        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        Task<GenerateNoModel> GeneratePaymentVoucherNo(int companyId, int financialYearId);

        Task<int> CreatePaymentVoucher(PaymentVoucherModel paymentVoucherModel);

        Task<bool> UpdatePaymentVoucher(PaymentVoucherModel paymentVoucherModel);

        Task<bool> DeletePaymentVoucher(int paymentVoucherId);

        Task<bool> UpdatePaymentVoucherMasterAmount(int? paymentVoucherId);

        Task<PaymentVoucherModel> GetPaymentVoucherById(int paymentVoucherId);
        
        /// <summary>
        /// get search  invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<PaymentVoucherModel>> GetPaymentVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterPaymentVoucherModel searchFilterModel);
   
        //Task<DataTableResultModel<PaymentVoucherModel>> GetPaymentVoucherList();
    }
}
