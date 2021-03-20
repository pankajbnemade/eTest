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
        IList<SelectListModel> GetTaxModelTypeSelectList();
        
        IList<SelectListModel> GetDiscountTypeSelectList();
        
        IList<SelectListModel> GetTaxPercentageOrAmountSelectList();

        IList<SelectListModel> GetTaxAddOrDeductSelectList();
    }
}
