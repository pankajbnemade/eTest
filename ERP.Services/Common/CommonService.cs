using ERP.Models.Helpers;
using ERP.Services.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Services.Common
{
    public class CommonService : ICommon
    {
        public IList<SelectListModel> GetTaxModelTypeSelectList()
        {
            IList<SelectListModel> resultModel = null;

            resultModel.Add(new SelectListModel { DisplayText = "Sub Total", Value = "Sub Total" });
            resultModel.Add(new SelectListModel { DisplayText = "Line Wise", Value = "Line Wise" });
                
            return resultModel; // returns.
        }

        public IList<SelectListModel> GetDiscountTypeSelectList()
        {
            IList<SelectListModel> resultModel = null;

            resultModel.Add(new SelectListModel { DisplayText = "Percentage", Value = "Percentage" });
            resultModel.Add(new SelectListModel { DisplayText = "Amount", Value = "Amount" });

            return resultModel; // returns.
        }

        public IList<SelectListModel> GetTaxPercentageOrAmountSelectList()
        {
            IList<SelectListModel> resultModel = null;

            resultModel.Add(new SelectListModel { DisplayText = "Percentage", Value = "Percentage" });
            resultModel.Add(new SelectListModel { DisplayText = "Amount", Value = "Amount" });

            return resultModel; // returns.
        }

        public IList<SelectListModel> GetTaxAddOrDeductSelectList()
        {
            IList<SelectListModel> resultModel = null;

            resultModel.Add(new SelectListModel { DisplayText = "Add", Value = "Add" });
            resultModel.Add(new SelectListModel { DisplayText = "Deduct", Value = "Deduct" });

            return resultModel; // returns.
        }

    }
}
