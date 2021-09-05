using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface IReceiptVoucher : IRepository<Receiptvoucher>
    {
        /// <summary>
        /// generate invoice no.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="financialYearId"></param>
        /// <returns>
        /// return invoice no.
        /// </returns>
        Task<GenerateNoModel> GenerateReceiptVoucherNo(int companyId, int financialYearId);

        Task<int> CreateReceiptVoucher(ReceiptVoucherModel receiptVoucherModel);

        Task<bool> UpdateReceiptVoucher(ReceiptVoucherModel receiptVoucherModel);

        Task<bool> DeleteReceiptVoucher(int receiptVoucherId);

        Task<bool> UpdateReceiptVoucherMasterAmount(int? receiptVoucherId);

        Task<ReceiptVoucherModel> GetReceiptVoucherById(int receiptVoucherId);
        
        /// <summary>
        /// get search  invoice result list.
        /// </summary>
        /// <param name="dataTableAjaxPostModel"></param>
        /// <param name="searchFilterModel"></param>
        /// <returns>
        /// return list.
        /// </returns>
        Task<DataTableResultModel<ReceiptVoucherModel>> GetReceiptVoucherList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterReceiptVoucherModel searchFilterModel);
   
        //Task<DataTableResultModel<ReceiptVoucherModel>> GetReceiptVoucherList();
    }
}
