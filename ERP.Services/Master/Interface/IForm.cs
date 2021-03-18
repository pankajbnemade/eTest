using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IForm : IRepository<Form>
    {
        Task<int> CreateForm(FormModel formModel);

        Task<bool> UpdateForm(FormModel formModel);

        Task<bool> DeleteForm(int formId);
       
        Task<FormModel> GetFormById(int formId);

        //Task<IList<FormModel>> GetFormByModuleId(int moduleId);

        Task<DataTableResultModel<FormModel>> GetFormList();
    }
}
