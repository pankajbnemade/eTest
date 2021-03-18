using ERP.DataAccess.EntityModels;
using ERP.Models.Master;
using ERP.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IChargeType : IRepository<Chargetype>
    {

        
        Task<int> CreateChargeType(ChargeTypeModel chargeTypeModel);

       
        Task<bool> UpdateChargeType(ChargeTypeModel chargeTypeModel);

       
        Task<bool> DeleteChargeType(int chargeTypeId);

       
        Task<ChargeTypeModel> GetChargeTypeById(int chargeTypeId);

        
        Task<DataTableResultModel<ChargeTypeModel>> GetChargeTypeList();

    }
}
