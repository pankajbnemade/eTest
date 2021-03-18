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
    public class VoucherSetupService : Repository<Vouchersetup>, IVoucherSetup
    {
        public VoucherSetupService(ErpDbContext dbContext) : base(dbContext) { }

        public async Task<int> CreateVoucherSetup(VoucherSetupModel voucherSetupModel)
        {
            int voucherSetupId = 0;

            // assign values.
            Vouchersetup voucherSetup = new Vouchersetup();

            voucherSetup.VoucherSetupName = voucherSetupModel.VoucherSetupName;
          
            voucherSetupId = await Create(voucherSetup);

            return voucherSetupId; // returns.
        }

        public async Task<bool> UpdateVoucherSetup(VoucherSetupModel voucherSetupModel)
        {
            bool isUpdated = false;

            // get record.
            Vouchersetup voucherSetup = await GetByIdAsync(w => w.VoucherSetupId == voucherSetupModel.VoucherSetupId);
            if (null != voucherSetup)
            {
                // assign values.
                voucherSetup.VoucherSetupName = voucherSetupModel.VoucherSetupName;
               
                isUpdated = await Update(voucherSetup);
            }

            return isUpdated; // returns.
        }


        public async Task<bool> DeleteVoucherSetup(int voucherSetupId)
        {
            bool isDeleted = false;

            // get record.
            Vouchersetup voucherSetup = await GetByIdAsync(w => w.VoucherSetupId == voucherSetupId);
            if (null != voucherSetup)
            {
                isDeleted = await Delete(voucherSetup);
            }

            return isDeleted; // returns.
        }


        public async Task<VoucherSetupModel> GetVoucherSetupById(int voucherSetupId)
        {
            VoucherSetupModel voucherSetupModel = null;

            IList<VoucherSetupModel> voucherSetupModelList = await GetVoucherSetupList(voucherSetupId);
            if (null != voucherSetupModelList && voucherSetupModelList.Any())
            {
                voucherSetupModel = voucherSetupModelList.FirstOrDefault();
            }

            return voucherSetupModel; // returns.
        }

        public async Task<DataTableResultModel<VoucherSetupModel>> GetVoucherSetupList()
        {
            DataTableResultModel<VoucherSetupModel> resultModel = new DataTableResultModel<VoucherSetupModel>();

            IList<VoucherSetupModel> voucherSetupModelList = await GetVoucherSetupList(0);
            if (null != voucherSetupModelList && voucherSetupModelList.Any())
            {
                resultModel = new DataTableResultModel<VoucherSetupModel>();
                resultModel.ResultList = voucherSetupModelList;
                resultModel.TotalResultCount = voucherSetupModelList.Count();
            }

            return resultModel; // returns.
        }


        /// <summary>
        /// get all voucherSetup list.
        /// </summary>
        /// <returns>
        /// return list.
        /// </returns>
        public async Task<IList<VoucherSetupModel>> GetVoucherSetupList(int voucherSetupId)
        {
            IList<VoucherSetupModel> voucherSetupModelList = null;

            // get records by query.

            IQueryable<Vouchersetup> query = GetQueryByCondition(w => w.VoucherSetupId != 0);

            if (0 != voucherSetupId)
                query = query.Where(w => w.VoucherSetupId == voucherSetupId);

            IList<Vouchersetup> voucherSetupList = await query.ToListAsync();

            if (null != voucherSetupList && voucherSetupList.Count > 0)
            {
                voucherSetupModelList = new List<VoucherSetupModel>();
                foreach (Vouchersetup voucherSetup in voucherSetupList)
                {
                    voucherSetupModelList.Add(await AssignValueToModel(voucherSetup));
                }
            }

            return voucherSetupModelList; // returns.
        }

        private async Task<VoucherSetupModel> AssignValueToModel(Vouchersetup voucherSetup)
        {
            return await Task.Run(() =>
            {
                VoucherSetupModel voucherSetupModel = new VoucherSetupModel();

                voucherSetupModel.VoucherSetupId = voucherSetup.VoucherSetupId;
                voucherSetupModel.VoucherSetupName = voucherSetup.VoucherSetupName;

                return voucherSetupModel;
            });
        }

    }
}
