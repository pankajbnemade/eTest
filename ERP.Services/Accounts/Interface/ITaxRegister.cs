using ERP.DataAccess.EntityModels;
using ERP.Models.Accounts;
using ERP.Models.Common;
using ERP.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Accounts.Interface
{
    public interface ITaxRegister : IRepository<Taxregister>
    {
        Task<int> CreateTaxRegister(TaxRegisterModel taxRegisterModel);

        Task<bool> UpdateTaxRegister(TaxRegisterModel taxRegisterModel);

        Task<bool> DeleteTaxRegister(int taxRegisterId);

        Task<TaxRegisterModel> GetTaxRegisterById(int taxRegisterId);

        //Task<DataTableResultModel<TaxRegisterModel>> GetTaxRegisterList();

        Task<DataTableResultModel<TaxRegisterModel>> GetTaxRegisterList(DataTableAjaxPostModel dataTableAjaxPostModel, SearchFilterTaxRegisterModel searchFilterModel);

        Task<IList<SelectListModel>> GetTaxRegisterSelectList();

    }
}
