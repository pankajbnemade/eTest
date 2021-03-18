using ERP.DataAccess.EntityData;
using ERP.DataAccess.EntityModels;
using ERP.Models.Common;
using ERP.Models.Master;
using ERP.Services.Master.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Master
{
    public class ChargeTypeService : Repository<Chargetype>, IChargeType
    {
        public ChargeTypeService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateChargeType(ChargeTypeModel chargeTypeModel)
        {
            int chargeTypeId = 0;

            // assign values.
            Chargetype chargeType = new Chargetype();

            chargeType.ChargeTypeName = chargeTypeModel.ChargeTypeName;
          

            chargeTypeId = await Create(chargeType);

            return chargeTypeId; // returns.
        }

        /// <summary>
        /// update chargeType.
        /// </summary>
        /// <param name="chargeTypeModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> UpdateChargeType(ChargeTypeModel chargeTypeModel)
        {
            bool isUpdated = false;

            // get record.
            Chargetype chargeType = await GetByIdAsync(w => w.ChargeTypeId == chargeTypeModel.ChargeTypeId);
            if (null != chargeType)
            {
                // assign values.
                chargeType.ChargeTypeName = chargeTypeModel.ChargeTypeName;
              

                isUpdated = await Update(chargeType);
            }

            return isUpdated; // returns.
        }

        /// <summary>
        /// update chargeType.
        /// </summary>
        /// <param name="chargeTypeModel"></param>
        /// <returns>
        /// return true if success.
        /// </returns>
        public async Task<bool> DeleteChargeType(int chargeTypeId)
        {
            bool isDeleted = false;

            // get record.
            Chargetype chargeType = await GetByIdAsync(w => w.ChargeTypeId == chargeTypeId);
            if (null != chargeType)
            {
                isDeleted = await Delete(chargeType);
            }

            return isDeleted; // returns.
        }

        /// <summary>
        /// get chargeType based on chargeTypeId
        /// </summary>
        /// <returns>
        /// return record.
        /// </returns>
        public async Task<ChargeTypeModel> GetChargeTypeById(int chargeTypeId)
        {
            ChargeTypeModel chargeTypeModel = null;

            IList<ChargeTypeModel> chargeTypeModelList = await GetChargeTypeList(chargeTypeId);
            if (null != chargeTypeModelList && chargeTypeModelList.Any())
            {
                chargeTypeModel = chargeTypeModelList.FirstOrDefault();
            }

            return chargeTypeModel; // returns.
        }


        /// <summary>
        /// get all chargeType list
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<DataTableResultModel<ChargeTypeModel>> GetChargeTypeList()
        {
            DataTableResultModel<ChargeTypeModel> resultModel = new DataTableResultModel<ChargeTypeModel>();

            IList<ChargeTypeModel> chargeTypeModelList = await GetChargeTypeList(0);
            if (null != chargeTypeModelList && chargeTypeModelList.Any())
            {
                resultModel = new DataTableResultModel<ChargeTypeModel>();
                resultModel.ResultList = chargeTypeModelList;
                resultModel.TotalResultCount = chargeTypeModelList.Count();
            }

            return resultModel; // returns.
        }

        private async Task<IList<ChargeTypeModel>> GetChargeTypeList(int chargeTypeId)
        {
            IList<ChargeTypeModel> chargeTypeModelList = null;

            // create query.
            IQueryable<Chargetype> query = GetQueryByCondition(w => w.ChargeTypeId != 0);

            // apply filters.
            if (0 != chargeTypeId)
                query = query.Where(w => w.ChargeTypeId == chargeTypeId);

            // get records by query.
            List<Chargetype> chargeTypeList = await query.ToListAsync();
            if (null != chargeTypeList && chargeTypeList.Count > 0)
            {
                chargeTypeModelList = new List<ChargeTypeModel>();
                foreach (Chargetype chargeType in chargeTypeList)
                {
                    chargeTypeModelList.Add(await AssignValueToModel(chargeType));
                }
            }

            return chargeTypeModelList; // returns.
        }

        private async Task<ChargeTypeModel> AssignValueToModel(Chargetype chargeType)
        {
            return await Task.Run(() =>
            {
                ChargeTypeModel chargeTypeModel = new ChargeTypeModel();

                // assign values.
                Chargetype chargeType = new Chargetype();

                chargeTypeModel.ChargeTypeName = chargeType.ChargeTypeName;
                
                return chargeTypeModel;
            });
        }
    }
}
