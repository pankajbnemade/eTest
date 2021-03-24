using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Helpers;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IDesignation : IRepository<Designation>
    {
       
        Task<int> CreateDesignation(DesignationModel designationModel);

        Task<bool> UpdateDesignation(DesignationModel designationModel);

        Task<bool> DeleteDesignation(int designationId);
       
        Task<DesignationModel> GetDesignationById(int designationId);
        
        Task<DataTableResultModel<DesignationModel>> GetDesignationList();

        Task<IList<SelectListModel>> GetDesignationSelectList();

    }
}
