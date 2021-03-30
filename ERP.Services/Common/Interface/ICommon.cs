using ERP.Models.Common;
using ERP.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Common.Interface
{
    public interface ICommon
    {
        Task<GenerateNoModel> GenerateVoucherNo(int maxNo, int voucherSetupId, int companyId, int financialYearId);

        Task<IList<SelectListModel>> GetTaxModelTypeSelectList();

        Task<IList<SelectListModel>> GetDiscountTypeSelectList();

        Task<IList<SelectListModel>> GetTaxPercentageOrAmountSelectList();

        Task<IList<SelectListModel>> GetTaxAddOrDeductSelectList();

        Task<string> AmountInWord_Million(string amount, string currencyCode, string denomination);
    }
}
