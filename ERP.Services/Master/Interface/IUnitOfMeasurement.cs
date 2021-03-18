using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Services.Master.Interface
{
    public interface IUnitOfMeasurement : IRepository<Unitofmeasurement>
    {
        Task<int> CreateUnitOfMeasurement(UnitOfMeasurementModel unitOfMeasurementModel);

        Task<bool> UpdateUnitOfMeasurement(UnitOfMeasurementModel unitOfMeasurementModel);

        Task<bool> DeleteUnitOfMeasurement(int unitOfMeasurementId);
       
        Task<UnitOfMeasurementModel> GetUnitOfMeasurementById(int unitOfMeasurementId);
        
        Task<DataTableResultModel<UnitOfMeasurementModel>> GetUnitOfMeasurementList();
    }
}
